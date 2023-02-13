import { Component } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import { AuthService } from '../serviceAuthorize/auth.service';
import { UserStoreService } from '../serviceAuthorize/user-store.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  loginForm!: FormGroup;
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private auth: AuthService,
    private userStore: UserStoreService,
    private toast: NgToastService
  ) {}
  isText: boolean = false;
  eyeIcon: string = 'fa-eye-slash';
  type: string = 'password';
  ngOnInit(): void {
    this.loginForm = this.fb.group({
      userName: ['', Validators.required],
      userPassword: ['', Validators.required],
    });
  }
  hideShowPass() {
    this.isText = !this.isText;
    this.isText ? (this.eyeIcon = 'fa-eye') : (this.eyeIcon = 'fa-eye-slash');
    this.isText ? (this.type = 'text') : (this.type = 'password');
  }
  onLogin() {
    console.log('alads');
    if (this.loginForm.valid) {
      //Send the obj to db
      console.log(this.loginForm.value);
      this.auth.login(this.loginForm.value).subscribe({
        next: (res: any) => {
          console.log(res.message);
          this.loginForm.reset();
          this.auth.storeToken(res.token);
          let tokenPayload = this.auth.decodeToken();
          console.log(tokenPayload)
          this.userStore.setFullNameForStore(tokenPayload.sub);
          this.userStore.setRoleforStore(tokenPayload.typ);
          this.auth.storeToken(res.token); //luu token vao localstorage
          this.toast.success({
            detail: 'Success',
            summary: 'You have loggin',
            duration: 5000,
          });
          this.router.navigate(['../home/dashboard']); // chinh sau
        },
        error: (err: any) => {
          this.toast.warning({
            detail: 'Warning',
            summary: 'Đã xảy ra một số lỗi có thể mật khẩu hoặc tên sai',
            duration: 5000,
          });
          console.log(err);
        },
      });
    } else {
      console.log('Form is not valid');
      //throw the error using toaster and with required files
    }
  }
  // onLogin() {
  //   if(this.loginForm.valid){
  //     console.log(this.loginForm.value)
  //     this.auth.login(this.loginForm.value).subscribe({next : (res)=>{
  //       alert(res.message)
  //       this.loginForm.reset();
  //       this.router.navigate(['dashboard'])
  //     }})
  //   }
  // }
  private validateAllFormFileds(formGroup: FormGroup) {
    Object.keys(formGroup.controls).forEach((field) => {
      const control = formGroup.get(field);
      if (control instanceof FormControl) {
        control.markAsDirty({ onlySelf: true });
      } else if (control instanceof FormGroup) {
        this.validateAllFormFileds(control);
      }
    });
  }
}
