import { Component, OnInit } from '@angular/core';
import { NgToastService } from 'ng-angular-popup';
import { BehaviorSubject } from 'rxjs';
import { ApiService } from '../Authorize/serviceAuthorize/api.service';
import { AuthService } from '../Authorize/serviceAuthorize/auth.service';
import { UserStoreService } from '../Authorize/serviceAuthorize/user-store.service';
// import { AuthService } from '../login/auth.service';
// import { AuthdataService } from '../login/authdata.service';

@Component({
  selector: 'app-main-screen',
  templateUrl: './main-screen.component.html',
  styleUrls: ['./main-screen.component.css']
})
export class MainScreenComponent implements OnInit {
  isMenuOpen = false;

  toggleMenu(): void {
    this.isMenuOpen = !this.isMenuOpen;
  }
public users:any = [];
public fullName : any;
public role !: number  ;
// public fullName$ = new BehaviorSubject<string>("");
// public role$ = new BehaviorSubject<string>("")
  constructor( private auth : AuthService,private api : ApiService, private userStore : UserStoreService,private toast : NgToastService) { }
  ngOnInit(): void {
    //lay thong tin user trong stoarge qua auth service
    this.api.getUsers().subscribe(res => {
      this.users  = res ;
    });
    this.userStore.getFullNameFromStore().subscribe((val : any) => {
      const fullNameFromToken = this.auth.getfullNameFromToken();
      this.fullName = val|| fullNameFromToken  ;
     
    })
    this.userStore.getRoleFromStore().subscribe((val : any) => {
      const rollFromToken = this.auth.getRoleFromToken();
      this.role =  val || rollFromToken
      
      console.log(this.role)
    })
  }
  
  logout(){
    this.auth.signOut()
    this.toast.success({detail : "Success",summary :"Bạn đã đăng xuất khỏi server của tôi ", duration : 5000});
  }
}
