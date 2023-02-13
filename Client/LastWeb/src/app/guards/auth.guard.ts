import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import { Observable } from 'rxjs';
import { AuthService } from '../routes/Authorize/serviceAuthorize/auth.service';
import { UserStoreService } from '../routes/Authorize/serviceAuthorize/user-store.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  role : any ;
  constructor(private userStore : UserStoreService,private auth : AuthService, private router: Router,private toast : NgToastService){}
  canActivate():boolean{
    if(this.auth.isLoggedIn()){
      return true
    }else{
      this.toast.error({detail:"ERROR",summary: "Please Login First !"})
      this.router.navigate(['login'])
      return false
    }
  }
  
  
}
