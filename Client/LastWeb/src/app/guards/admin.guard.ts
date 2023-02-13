import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import { Observable } from 'rxjs';
import { AuthService } from '../routes/Authorize/serviceAuthorize/auth.service';
import { UserStoreService } from '../routes/Authorize/serviceAuthorize/user-store.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
  fullName : any
  constructor(private userStore : UserStoreService,private auth : AuthService, private router: Router,private toast : NgToastService){}

  canActivate() : boolean{

    this.userStore.getFullNameFromStore().subscribe((val : any) => {
      const fullNameFromToken = this.auth.getfullNameFromToken();
      this.fullName = val|| fullNameFromToken  ;
     
    })
    
    if(this.fullName === "Huy Tuan"  && this.auth.isLoggedIn() ){
      return true
    }else 
    {
      this.toast.warning({detail:"Warning",summary: "Bạn không có quyền truy cập trang này !"})
      return false ;
    }
  }
  
}
