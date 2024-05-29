import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { LangChangeEvent, TranslateService } from '@ngx-translate/core';
import { interval, Subject, debounce, finalize, Observable, fromEvent, catchError, throwError, delay, retryWhen } from 'rxjs';
import { User, UserFilter } from '../../models/user';
import { AppConfigService } from '../../services/app-config.service';
import { BulkDeleteUserDialogComponent } from '../bulk-delete-user-dialog/bulk-delete-user-dialog.component';
import { CreateUserDialogComponent } from '../create-user-dialog/create-user-dialog.component';

@Component({
  selector: 'app-module-user',
  templateUrl: './module-user.component.html',
  styleUrls: ['./module-user.component.scss']
})
export class ModuleUserComponent implements OnInit {

  URL_USER_LIST = '/api/user/user-list';

  keyword: string = '';
  pageSize: number = 30;
  sortBy: string = 'name';
  sortByDisplayName: string = this.getSortByName();
  ascSort: boolean = true;

  currentPageIndex: number = 0;
  showLoadMore: boolean = true;

  status: string = 'all';
  users: User[] = [];
  isLoading: boolean = false;
  isEmpty: boolean = false;
  isCtrlPressed: boolean = false;
  selectedUser: User[] = [];

  private searchSubject = new Subject<string>();
  private inViewSubject = new Subject<string>();
  private scrollSubject = new Subject<string>();

  private lastRecTop: number = 0;
  private currentRecTop: number = 0;

  @ViewChild('loadMore', { static: false }) private loadMoreDiv: ElementRef<HTMLDivElement> = {} as ElementRef<HTMLDivElement>;
 
  constructor(
    public appConfig: AppConfigService,
    public dialog: MatDialog,
    public http: HttpClient,
    public snackBar: MatSnackBar,
    public translate: TranslateService,
  ) {

  }

  ngOnInit(): void {
    if (this.appConfig.isLoaded) {
      this.loadUserList(0);
    } else {
      this.appConfig.onAppConfigLoaded().subscribe(() => {
        this.loadUserList(0);
      })
    }

    this.searchSubject.pipe(
      debounce(() => interval(600))
    ).subscribe(() => {
      this.loadUserList(0);
    });

    this.inViewSubject.pipe(
      debounce(() => interval(600))
    ).subscribe((msg) => {
      this.loadUserList(this.currentPageIndex);
    });

    fromEvent(window, 'wheel').subscribe(() => {
      this.scrollSubject.next('scroll ' + new Date().getTime());
    });

    this.translate.onLangChange.subscribe((event: LangChangeEvent) => {
      this.sortByDisplayName = this.getSortByName();
    });

    this.getSortByName();
  }

  ngAfterViewInit() {
    this.scrollSubject.pipe(
      debounce(() => interval(100))
    ).subscribe((msg) => {
      console.log(msg);
      const isInView = this.checkLoadMoreInView();
      if (isInView) {
        const msg = 'in view ' + new Date().getTime();
        console.log(msg);
        this.inViewSubject.next(msg);
      }
    });

  }

  @HostListener('document:keydown', ['$event'])
  handleKeydown(event: KeyboardEvent) {
    console.log(event.key);
    if (event.key === 'Control') {
      this.isCtrlPressed = true;
    }
  }

  @HostListener('document:keyup', ['$event'])
  handleKeyup(event: KeyboardEvent) {
    this.isCtrlPressed = false;
  }

  onSortOrderChanged() {
    this.ascSort = !this.ascSort;
    this.loadUserList(0);
  }

  onSortByClicked(sortBy: string) {
    this.sortBy = sortBy;
    this.sortByDisplayName = this.getSortByName();   
    this.loadUserList(0);
  }

  getSortByName() {
    switch (this.sortBy) {
      case 'name':
        return this.translate.instant('MODULE_USER.NAME');
      case 'email':
        return this.translate.instant('MODULE_USER.EMAIL');
      case 'role':
        return this.translate.instant('MODULE_USER.ROLE');
      case 'last_sign_in':
        return this.translate.instant('MODULE_USER.LAST_SIGN_IN');
      case 'register_time':
        return this.translate.instant('MODULE_USER.REGISTER_TIME');
    }
  }

  checkLoadMoreInView(): boolean{
    if (this.loadMoreDiv) {
      const rect = this.loadMoreDiv.nativeElement.getBoundingClientRect();
      this.lastRecTop = this.currentRecTop;
      this.currentRecTop = rect.top;
      console.log('rect top:' + rect.top + ', rect bottom: ' + rect.bottom + ', inner height: ' + window.innerHeight);
      const isInView = rect.top < (window.innerHeight); //topShown && bottomShown;
      return isInView;
    } else {
      return false;
    }
  }

  clearKeyword() {
    this.keyword = '';
    this.loadUserList(0);
  }

  loadUserList(pageIndex:number) {
    console.log('load users');

    const url = this.appConfig.apiEndpoint + this.URL_USER_LIST;

    const filter: UserFilter = {
      pageIndex: pageIndex,
      pageSize: this.pageSize,
      sortBy: this.sortBy,
      ascSort: this.ascSort,
      keyword: this.keyword,
      isBlocked: null,
      role: ''
    }

    if (this.status === 'all')
      filter.isBlocked = null;
    else if (this.status === 'active')
      filter.isBlocked = false;
    else
      filter.isBlocked = true;

    this.isLoading = true;
    return this.http.post<User[]>(url, filter).pipe(
      debounce(() => interval(1000)),
      retryWhen((errors: Observable<any>) => errors.pipe(delay(10000))),
      catchError((error) => {
        this.snackBar.open(this.translate.instant('MODULE_USER.LOAD_DATA_ERROR'), '', {
          duration: 3000
        });
        return throwError(() => new Error(error.message));
      }),
      finalize(() => { this.isLoading = false; })
    ).subscribe((res) => {
      console.log('Page ' + pageIndex + ' - ' + res.length);

      if (pageIndex === 0) {
        this.users = [];
        this.selectedUser = [];
      }

      this.users.push(...res);

      if (this.users.length > 0)
        this.isEmpty = false;
      else
        this.isEmpty = true;

      if (res.length === this.pageSize) {
        this.currentPageIndex += 1;
        this.showLoadMore = true;
      } else {
        this.showLoadMore = false; //end of record
      }
      
    });
  }

  onStatusChanged(e: any) {
    this.loadUserList(0);
  }

  onSearch(e:any) {
    console.log(e);
    this.searchSubject.next(e);
  }

  openCreateUserDialog() {
    var dialogRef = this.dialog.open(CreateUserDialogComponent, { disableClose: false, width: '480px' });
    dialogRef.afterClosed().subscribe((msg) => {
      console.log(msg);
      if (msg === 'user-created') {
        this.loadUserList(0);
      }
    });
  }

  openBulkDeleteUserDialog() {
    var dialogRef = this.dialog.open(BulkDeleteUserDialogComponent, { disableClose: false, data: this.selectedUser, width: '480px' });
    dialogRef.afterClosed().subscribe((msg) => {
      if (msg === 'deleted') {
        this.users = this.users.filter((el)=> {
          return !this.selectedUser.includes(el);
        });
        this.selectedUser = [];
      }
    })
  }

  onUserDeleted(email:string) {
    console.log('user-deleted: ' + email);
    for (var i = 0; i < this.users.length; i++) {
      if (this.users[i].email === email) {
        this.users.splice(i, 1)
        return;
      }
    }
  }

  onItemClicked(email: string) {
    console.log('user-clicked: ' + email);

    for (var i = 0; i < this.users.length; i++) {
      if (this.users[i].email === email) {
        this.users[i].selected = !this.users[i].selected;
      } else {
        if (!this.isCtrlPressed) {
          this.users[i].selected = false;
        }
      }
    }

    let tmpSelectedUser:User[] = [];
    for (var i = 0; i < this.users.length; i++) {
      if (this.users[i].selected) {
        tmpSelectedUser.push(this.users[i]);
      }
    }

    this.selectedUser = tmpSelectedUser;
  }
}
