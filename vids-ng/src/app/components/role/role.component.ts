import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TranslateService } from '@ngx-translate/core';
import { interval, Subject, debounce, finalize, Observable, fromEvent, catchError, throwError } from 'rxjs';
import { Role, RoleCC } from '../../models/role';
import { AppConfigService } from '../../services/app-config.service';
import { UniqueRoleValidator } from 'src/app/validators/unique-role.validators';
import { DeleteRoleDialogComponent } from '../delete-role-dialog/delete-role-dialog.component';
import { RoleUsersDialogComponent } from '../role-users-dialog/role-users-dialog.component';

@Component({
  selector: 'app-role',
  templateUrl: './role.component.html',
  styleUrls: ['./role.component.scss']
})
export class RoleComponent implements OnInit {

  URL_ROLE = '/api/role/cc/role';
  URL_ROLE_LIST = '/api/role/cc/role-list';
  URL_UPDATE_ROLE = '/api/role/cc/update-role';
  URL_CREATE_ROLE = '/api/role/cc/create-role';

  role: RoleCC = new RoleCC();
  roleList: Role[] = [];

  isLoadingRole: boolean = false;
  isSaving: boolean = false;
  isLoadingRoles: boolean = false;
  isReadOnly: boolean = false;
  isDeleting: boolean = false;
  isNewRole: boolean = false;

  form: FormGroup = new FormGroup({});

  userCountTooltip: string = '';

  constructor(
    public fb: FormBuilder,
    public appConfig: AppConfigService,
    public http: HttpClient,
    public snackBar: MatSnackBar,
    public dialog: MatDialog,
    public uniqueCCRoleValidator: UniqueRoleValidator,
    public translate: TranslateService) {
    this.initForm();
    this.role.roleName = 'New Role';
    this.isNewRole = true;
  }

  ngOnInit(): void {
    if (this.appConfig.isLoaded) {
      this.loadRoleList();
    } else {
      this.appConfig.onAppConfigLoaded().subscribe(() => {
        this.loadRoleList();
      })
    }

    this.userCountTooltip = this.translate.instant('ROLE.USER_COUNT_SINGLE').replace('{0}', this.role.userCount);
  }

  createNewRole() {
    this.role = new RoleCC();
    this.isReadOnly = false;
    this.isNewRole = true;
    this.userCountTooltip = this.translate.instant('ROLE.USER_COUNT_SINGLE').replace('{0}', this.role.userCount);
    this.initForm();
  }

  initForm() {
    this.form = this.fb.group({
      oriRoleName: [this.role.roleName],
      roleName: [this.role.roleName, { validators: [Validators.required], asyncValidators: [this.uniqueCCRoleValidator] }],
      viewDashboardOperation: [{ value: this.role.viewDashboardOperation, disabled: this.isReadOnly }],
      viewDashboardOverall: [{ value: this.role.viewDashboardOverall, disabled: this.isReadOnly }],
      viewHighwayInfo: [{ value: this.role.viewHighwayInfo, disabled: this.isReadOnly }],
      viewPublicInfo: [{ value: this.role.viewPublicInfo, disabled: this.isReadOnly }],
      viewUserFeedback: [{ value: this.role.viewUserFeedback, disabled: this.isReadOnly }],
      viewHighwayRating: [{ value: this.role.viewHighwayRating, disabled: this.isReadOnly }],
      viewReportAuditLog: [{ value: this.role.viewReportAuditLog, disabled: this.isReadOnly }],
      viewReportHighwayInfoSummary: [{ value: this.role.viewReportHighwayInfoSummary, disabled: this.isReadOnly }],
      viewReportHighwayInfoComparison: [{ value: this.role.viewReportHighwayInfoComparison, disabled: this.isReadOnly }],
      viewReportPublicInfo: [{ value: this.role.viewReportPublicInfo, disabled: this.isReadOnly }],
      viewReportUserFeedback: [{ value: this.role.viewReportUserFeedback, disabled: this.isReadOnly }],
      viewAdminAreaUser: [{ value: this.role.viewAdminAreaUser, disabled: this.isReadOnly }],
      viewAdminAreaRole: [{ value: this.role.viewAdminAreaRole, disabled: this.isReadOnly }],
      viewAdminAreaHighway: [{ value: this.role.viewAdminAreaHighway, disabled: this.isReadOnly }],
      viewAdminAreaAgency: [{ value: this.role.viewAdminAreaAgency, disabled: this.isReadOnly }],
      editAdminAreaUser: [{ value: this.role.editAdminAreaUser, disabled: this.isReadOnly }],
      editAdminAreaRole: [{ value: this.role.editAdminAreaRole, disabled: this.isReadOnly }],
      editAdminAreaAgency: [{ value: this.role.editAdminAreaAgency, disabled: this.isReadOnly }],
      editAdminAreaHighway: [{ value: this.role.editAdminAreaHighway, disabled: this.isReadOnly }],
      editUserFeedback: [{ value: this.role.editUserFeedback, disabled: this.isReadOnly }],
      editHighwayInfo: [{ value: this.role.editHighwayInfo, disabled: this.isReadOnly }],
      editPublicInfo: [{ value: this.role.editPublicInfo, disabled: this.isReadOnly }],
      editHighwayRating: [{ value: this.role.editHighwayRating, disabled: this.isReadOnly }]
    });
  }

  loadRole(roleName: string) {
    console.log('load role ' + roleName);

    const url = this.appConfig.apiEndpoint + this.URL_ROLE;
    const params = {
      'roleName': roleName
    };

    this.isLoadingRole = true;
    return this.http.get<RoleCC>(url, { 'params': params }).pipe(
      finalize(() => { this.isLoadingRole = false; })
    ).subscribe((res) => {
      this.role = res as RoleCC;

      console.log(this.role);

      if (this.role.roleName === 'Admin' || this.role.roleName === 'Owner')
        this.isReadOnly = true;
      else
        this.isReadOnly = false;

      this.isNewRole = false;

      if (this.role.userCount > 1) {
        this.userCountTooltip = this.translate.instant('ROLE.USER_COUNT_MANY').replace('{0}', this.role.userCount);
      } else {
        this.userCountTooltip = this.translate.instant('ROLE.USER_COUNT_SINGLE').replace('{0}', this.role.userCount);
      }
      this.initForm();
    });
  }

  printRole(role: RoleCC) {
    console.log(role);
  }

  submitForm(role: RoleCC) {
    if (this.isNewRole) {
      this.createRole(role);
    } else {
      this.updateRole(role);
    }
  }

  createRole(role: RoleCC) {
    const url = this.appConfig.apiEndpoint + this.URL_CREATE_ROLE;

    this.isSaving = true;
    return this.http.post(url, role).pipe(
      catchError((error) => {
        this.snackBar.open(this.translate.instant('ROLE.CREATE_ROLE_ERROR'), '', {
          duration: 3000
        });
        return throwError(() => new Error(error.message));
      }),
      finalize(() => { this.isSaving = false; })
    ).subscribe((res) => {
      this.snackBar.open(this.translate.instant('ROLE.CREATE_ROLE_SUCCESS'), '', {
        duration: 3000
      });
      this.loadRole(role.roleName);
      this.loadRoleList();
    });
  }

  updateRole(role: RoleCC) {
    const url = this.appConfig.apiEndpoint + this.URL_UPDATE_ROLE;

    this.isSaving = true;
    return this.http.post(url, role).pipe(
      catchError((error) => {
        this.snackBar.open(this.translate.instant('ROLE.UPDATE_ROLE_ERROR'), '', {
          duration: 3000
        });
        return throwError(() => new Error(error.message));
      }),
      finalize(() => { this.isSaving = false; })
    ).subscribe((res) => {
      this.snackBar.open(this.translate.instant('ROLE.UPDATE_ROLE_SUCCESS'), '', {
        duration: 3000
      });
    });
  }

  loadRoleList() {
    console.log('load roles');

    const url = this.appConfig.apiEndpoint + this.URL_ROLE_LIST;

    this.isLoadingRoles = true;
    return this.http.get<Role[]>(url).pipe(
      finalize(() => { this.isLoadingRoles = false; })
    ).subscribe((res) => {
      this.roleList = res as Role[];
    });
  }

  onRoleClicked(roleName: string) {
    this.loadRole(roleName);
  }

  openDeleteRoleDialog() {
    if (this.role.userCount > 0) {
      this.snackBar.open(this.translate.instant('ROLE.USER_EXIST'), '', {
        duration: 3000
      });
    } else {
      var dialogRef = this.dialog.open(DeleteRoleDialogComponent, { disableClose: false, data: this.role, width: '480px' });
      dialogRef.afterClosed().subscribe((msg: string) => {
        if (msg === 'deleted') {
          this.roleList = this.roleList.filter((el) => {
            return !this.role.roleName.includes(el.roleName);
          });
          this.createNewRole();
        }
      })
    }
  }

  openRoleUsersDialog() {
    const tmpDialogRef = this.dialog.open(RoleUsersDialogComponent, { disableClose: false, data: this.role, width: '480px', autoFocus: false });
    tmpDialogRef.afterClosed().subscribe((msg) => {
      if (msg === 'deleted') {
        this.loadRole(this.role.roleName); //reload role to refresh the number of user count.
      }
    });
  }


}
