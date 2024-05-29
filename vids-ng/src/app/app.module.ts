import { APP_INITIALIZER, Injector, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { MatRippleModule } from '@angular/material/core';
import { MatSliderModule } from '@angular/material/slider';
import { MatButtonModule } from '@angular/material/button';
import { MatRadioModule } from '@angular/material/radio';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatMenuModule } from '@angular/material/menu';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTreeModule } from '@angular/material/tree';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatDialogModule } from '@angular/material/dialog';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatListModule } from '@angular/material/list';
import { MatCardModule } from '@angular/material/card';
import { MatBadgeModule } from '@angular/material/badge';

import { BoldPipe } from './pipes/bold.pipe';
import { ReplacePipe } from './pipes/replace.pipe';
import { DateTimePipe } from './pipes/dateTime.pipe';

import { AppComponent } from './app.component';
import { UpdatePasswordDialogComponent } from './components/update-password-dialog/update-password-dialog.component';
import { UpdateProfileDialogComponent } from './components/update-profile-dialog/update-profile-dialog.component';
import { UpdateAvatarDialogComponent } from './components/update-avatar-dialog/update-avatar-dialog.component';
import { CreateUserDialogComponent } from './components/create-user-dialog/create-user-dialog.component';
import { DeleteUserDialogComponent } from './components/delete-user-dialog/delete-user-dialog.component';
import { EditUserDialogComponent } from './components/edit-user-dialog/edit-user-dialog.component';
import { BulkDeleteUserDialogComponent } from './components/bulk-delete-user-dialog/bulk-delete-user-dialog.component';
import { UserCardComponent } from './components/user-card/user-card.component';
import { UserInfoDialogComponent } from './components/user-info-dialog/user-info-dialog.component';
import { UpdateUserPasswordDialogComponent } from './components/update-user-password-dialog/update-user-password-dialog.component';
import { UpdateUserInfoDialogComponent } from './components/update-user-info-dialog/update-user-info-dialog.component';
import { UpdateUserAvatarDialogComponent } from './components/update-user-avatar-dialog/update-user-avatar-dialog.component';
import { RoleComponent } from './components/role/role.component';
import { ShimmerTableComponent } from './components/shimmer-table/shimmer-table.component';
import { ShimmerFormComponent } from './components/shimmer-form/shimmer-form.component';
import { ShimmerListComponent } from './components/shimmer-list/shimmer-list.component';
import { ShimmerCardsComponent } from './components/shimmer-cards/shimmer-cards.component';
import { DeleteRoleDialogComponent } from './components/delete-role-dialog/delete-role-dialog.component';
import { RoleUsersDialogComponent } from './components/role-users-dialog/role-users-dialog.component';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { LayoutMainComponent } from './components/layout-main/layout-main.component';
import { ModuleRoleComponent } from './components/module-role/module-role.component';
import { ModuleUserComponent } from './components/module-user/module-user.component';
import { SignUpSuccessComponent } from './components/sign-up-success/sign-up-success.component';
import { ResetPasswordRequestedComponent } from './components/reset-password-requested/reset-password-requested.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { Error500Component } from './components/error-500/error-500.component';
import { Error404Component } from './components/error-404/error-404.component';
import { LayoutGeneralComponent } from './components/layout-general/layout-general.component';
import { Error403Component } from './components/error-403/error-403.component';
import { SignUpFormComponent } from './components/sign-up-form/sign-up-form.component';
import { ForgotPasswordFormComponent } from './components/forgot-password-form/forgot-password-form.component';
import { LoginFormComponent } from './components/login-form/login-form.component';
import { TranslateLoader, TranslateModule, TranslateService } from '@ngx-translate/core';
import { AppRoutingModule } from './app-routing.module';
import { LOCATION_INITIALIZED } from '@angular/common';
import { AppConfigService } from './services/app-config.service';
import { AppService } from './services/app.service';
import { TokenService } from './services/token.service';
import { CustomHttpInterceptor } from './interceptors/custom-http.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    LoginFormComponent,
    ForgotPasswordFormComponent,
    SignUpFormComponent,
    LayoutMainComponent,
    LayoutGeneralComponent,
    Error403Component,
    Error500Component,
    Error404Component,
    ResetPasswordComponent,
    ResetPasswordRequestedComponent,
    BoldPipe,
    ReplacePipe,
    DateTimePipe,
    SignUpSuccessComponent,
    ModuleUserComponent,
    ModuleRoleComponent,
    UpdatePasswordDialogComponent,
    UpdateProfileDialogComponent,
    UpdateAvatarDialogComponent,
    CreateUserDialogComponent,
    DeleteUserDialogComponent,
    EditUserDialogComponent,
    BulkDeleteUserDialogComponent,
    UserCardComponent,
    UserInfoDialogComponent,
    UpdateUserPasswordDialogComponent,
    UpdateUserInfoDialogComponent,
    UpdateUserAvatarDialogComponent,
    RoleComponent,
    ShimmerTableComponent,
    ShimmerFormComponent,
    ShimmerListComponent,
    ShimmerCardsComponent,
    DeleteRoleDialogComponent,
    RoleUsersDialogComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      },
      defaultLanguage: 'en', //default language set to english
    }),
    FormsModule,
    ReactiveFormsModule,
    MatSliderModule,
    MatButtonModule,
    MatRadioModule,
    MatTooltipModule,
    MatMenuModule,
    MatRippleModule,
    MatDividerModule,
    MatIconModule,
    MatInputModule,
    MatCheckboxModule,
    MatSnackBarModule,
    MatProgressSpinnerModule,
    MatSidenavModule,
    MatTreeModule,
    MatToolbarModule,
    MatDialogModule,
    MatProgressBarModule,
    MatSelectModule,
    MatListModule,
    MatSlideToggleModule,
    MatCardModule,
    MatBadgeModule
  ],
  providers: [
    AppService,
    TokenService,
    {
      provide: APP_INITIALIZER,
      multi: true,
      deps: [AppConfigService],
      useFactory: (appConfigService: AppConfigService) => () => { appConfigService.loadAppConfig(); }
    },
    {
      provide: APP_INITIALIZER,
      multi: true,
      deps: [TokenService],
      useFactory: (tokenService: TokenService) => () => { tokenService.loadAccessToken(); tokenService.loadRefreshToken(); }
    },
    {
      provide: APP_INITIALIZER,
      useFactory: TranslateFactory,
      deps: [TranslateService, Injector],
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: CustomHttpInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

//#region translation

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}

export function TranslateFactory(translate: TranslateService, injector: Injector) {
  return async () => {
    await injector.get(LOCATION_INITIALIZED, Promise.resolve(null));

    const deaultLang = 'en';
    translate.addLangs(['en', 'ms', 'zh']);
    translate.setDefaultLang(deaultLang);
    try {
      await translate.use(deaultLang).toPromise();
    } catch (err) {
      console.log(err);
    }
  };
}

//#endregion
