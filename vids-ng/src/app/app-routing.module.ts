import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SignUpSuccessComponent } from './components/sign-up-success/sign-up-success.component';
import { Error500Component } from './components/error-500/error-500.component';
import { Error404Component } from './components/error-404/error-404.component';
import { Error403Component } from './components/error-403/error-403.component';
import { LayoutGeneralComponent } from './components/layout-general/layout-general.component';
import { LayoutMainComponent } from './components/layout-main/layout-main.component';
import { ForgotPasswordFormComponent } from './components/forgot-password-form/forgot-password-form.component';
import { LoginFormComponent } from './components/login-form/login-form.component';
import { ResetPasswordRequestedComponent } from './components/reset-password-requested/reset-password-requested.component';
import { SignUpFormComponent } from './components/sign-up-form/sign-up-form.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { AuthGuardService } from './guards/auth-guard.service';
import { PermissionGuardService } from './guards/permission-guard.service';
import { ModuleUserComponent } from './components/module-user/module-user.component';
import { ModuleRoleComponent } from './components/module-role/module-role.component';

const routes: Routes = [
  {path: '', component: LayoutGeneralComponent, children:[
    {path: '', redirectTo:'/main', pathMatch: 'full'},
    {path: 'login', component: LoginFormComponent},   
    {path: 'sign-up', component: SignUpFormComponent},   
    {path: 'sign-up-success', component: SignUpSuccessComponent},
    {path: 'forgot-password', component: ForgotPasswordFormComponent},
    {path: 'reset-password-requested', component: ResetPasswordRequestedComponent},
    {path: 'reset-password', component: ResetPasswordComponent}
  ]},
  {path:'main', component: LayoutMainComponent, canActivate: [AuthGuardService], children:[
    {path:'admin-area/user', component: ModuleUserComponent},
    {path:'admin-area/role', component: ModuleRoleComponent},
  ]},
  {path: 'error', component: LayoutGeneralComponent, children:[
    {path: 'unauthorized', component:Error403Component},
    {path: 'internal-server-error', component: Error500Component},
    {path: 'not-found', component: Error404Component}
  ]},
  { path: '**', redirectTo: '/error/not-found', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
