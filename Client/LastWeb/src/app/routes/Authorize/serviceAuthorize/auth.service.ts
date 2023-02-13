import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { catchError, Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = 'http://localhost:43268/api/Account/';
private usePayload :any
  constructor(private httpClient: HttpClient,private router : Router) {
    this.usePayload = this.decodeToken();
  }

  public authLogin(username : string, password : any): Observable<any> {
    console.log('AuthService: authLogin: ', username, password);
    const httpParams = new HttpParams();
    const payload = { username, password };
    return (
      this.httpClient
        .post(this.baseUrl, payload, {
          params: httpParams,
        })
        // .pipe(delay(3000))
        // .pipe(map(data => {
        //   console.log('DataService: login', data);
        //   return data;
        // }))
        .pipe(catchError(this.handleError))
    );
  }
  login(loginObj :any){
    return this.httpClient.post<any>(`${this.baseUrl}Login`,loginObj)
  }
signUp(userObj : any){
  return this.httpClient.post<any>(`${this.baseUrl}Register`,userObj)
}
isLoggedIn() : boolean {
  return !!localStorage.getItem('token')
}
signOut(){
  localStorage.clear();
  this.router.navigate(['login'])
}
storeToken(tokenValue : string){
  localStorage.setItem('token',tokenValue)
}
getToken(){
  return localStorage.getItem('token');
}

  handleError(error: HttpErrorResponse): Observable<any> {
    let errorMessage = 'Unknown error!';
    if (error.error instanceof ErrorEvent) {
      // Client-side errors
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side errors
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    // window.alert(errorMessage);
    console.log('Error', errorMessage);
    return throwError(errorMessage);
  }
  decodeToken(){
    const jwtHelper = new JwtHelperService();
    const token = this.getToken()!;
    console.log(jwtHelper.decodeToken(token))
    return jwtHelper.decodeToken(token)
  }
  getfullNameFromToken(){
    if(this.usePayload){
      return this.usePayload.sub ;
    }
  }
  getRoleFromToken(){
    if(this.usePayload)
    return this.usePayload.typ
  }
  getUserIdFromToken(){

    if(this.usePayload){
      return this.usePayload.sid
    }
  }

}
