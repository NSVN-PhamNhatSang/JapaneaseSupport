import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import { AuthService } from '../serviceAuthorize/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  signUpForm!: FormGroup ;
  constructor( private fb : FormBuilder,private toast : NgToastService,private router : Router,private auth : AuthService) { }
  ngOnInit(): void {
    this.signUpForm = this.fb.group({
      userName : ['',Validators.required],
      userPassword : ['',Validators.required]
    })
    }

  onSignUp(){
    if(this.signUpForm.valid){
      this.auth.signUp(this.signUpForm.value).
      subscribe({next:(res :any) => {
        this.signUpForm.reset();
        this.toast.success({detail : "Success",summary : "", duration : 5000});
      }
      ,error:((err :any) =>{
      this.router.navigate(['../login'])
      this.toast.success({detail : "Success",summary :"Register have done", duration : 5000});

    })
    })
    console.log(this.signUpForm.value)
    }
   }
 }
