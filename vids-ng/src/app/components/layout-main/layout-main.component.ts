import { NestedTreeControl } from '@angular/cdk/tree';
import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatDivider } from '@angular/material/divider';
import { MatTreeNestedDataSource } from '@angular/material/tree';
import { TranslateService } from '@ngx-translate/core';
import { AppConfigService } from 'src/app/services/app-config.service';
import { TokenService } from 'src/app/services/token.service';
import { AppService } from '../../services/app.service';
import { UpdatePasswordDialogComponent } from '../update-password-dialog/update-password-dialog.component';
import { UpdateAvatarDialogComponent } from '../update-avatar-dialog/update-avatar-dialog.component';
import { UpdateProfileDialogComponent } from '../update-profile-dialog/update-profile-dialog.component';

interface Node {
  name: string;
  icon?: string;
  url?: string;
  children?: Node[];
}

@Component({
  selector: 'app-layout-main',
  templateUrl: './layout-main.component.html',
  styleUrls: ['./layout-main.component.scss']
})
export class LayoutMainComponent implements OnInit {
  treeControl = new NestedTreeControl<Node>(node => node.children);
  dataSource = new MatTreeNestedDataSource<Node>();
 
  constructor(public appService: AppService, 
    public translate: TranslateService, 
    public appConfig: AppConfigService, 
    public tokenService: TokenService,
    public dialog: MatDialog) {
    this.translate.use(this.appService.currentLanguage).subscribe(()=>{
      this.dataSource.data = this.setupTreenode();
    });
  }

  setupTreenode(): Node[]{
    let treeNodes: Node[] = [
      { name: this.translate.instant('SIDEBAR.DASHBOARD'), icon: 'dashboard', url: './dashboard'},
      { name: this.translate.instant('SIDEBAR.PLAYER'), icon: 'tv', url: './player' },
      { name: this.translate.instant('SIDEBAR.APP'), icon: 'apps', url: './app' },
      { name: this.translate.instant('SIDEBAR.TEMPLATE'), icon: 'view_compact', url: './template' },
      { name: this.translate.instant('SIDEBAR.CONTENT'), icon: 'app_registration', url: './content' },
      {
        name: this.translate.instant('SIDEBAR.REPORT'),
        icon: 'analytics',
        children: [
          { name: this.translate.instant('SIDEBAR.AUDIT_TRAILS'), url: './report/audit-trails'}, 
         ]
      },
      {
        name: this.translate.instant('SIDEBAR.ADMIN_AREA'),
        icon: 'settings',
        children: [
          { name: this.translate.instant('SIDEBAR.USER'), url: './admin-area/user'}, 
          { name: this.translate.instant('SIDEBAR.ROLE'), url: './admin-area/role' }]
      }
    ];

    return treeNodes;
  }

  hasChild = (_: number, node: Node) => !!node.children && node.children.length > 0;

  ngOnInit(): void {
    this.appService.getAvatar();
    this.appConfig.onAppConfigLoaded().subscribe(()=>{
      this.appService.getAvatar();
    });
  }

  toggleSidebar(){
    this.appService.isSidebarOn = !this.appService.isSidebarOn;
  }

  changeLanguage(lang: string){
    this.translate.use(lang).subscribe(()=>{
      this.dataSource.data = this.setupTreenode();
      this.appService.updateLanguage(lang);
    });
  }

  changeTheme(theme:string){
    this.appService.updateThemeColor(theme); 
  }

  changeMode(mode:string){
    this.appService.updateThemeMode(mode);
  }

  openUpdatePasswordDialog(){
     this.dialog.open(UpdatePasswordDialogComponent, {disableClose: false});
  }

  openUpdateProfileDialog(){
    this.dialog.open(UpdateProfileDialogComponent, {disableClose: false});
  }

  openUpdateAvatarDialog(){
    this.dialog.open(UpdateAvatarDialogComponent, {disableClose: false});
  }

  signOut(){
    this.appService.signOut();
  }
  
}
