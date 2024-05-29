import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { TokenService } from '../services/token.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService implements CanActivate {

  constructor(private tokenService: TokenService, private router: Router) { }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot ):| Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree { 
    let isAuthenticated = this.tokenService.checkIsAuthenticated();
    if(isAuthenticated){
      return true;
    }else {
      this.router.navigate(['login']);
      return false;
    }
 }
}
