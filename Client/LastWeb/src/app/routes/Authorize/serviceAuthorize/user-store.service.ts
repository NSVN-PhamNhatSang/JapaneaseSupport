import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserStoreService {
private fullName$ = new BehaviorSubject<string>("");
private role$ = new BehaviorSubject<string>("");
private id$ = new BehaviorSubject<any>("")
  constructor() { }
  public getRoleFromStore(){
    return this.role$.asObservable();
  }
  public setRoleforStore(role: any){
    this.role$.next(role);
  }
  public getFullNameFromStore(){
   return this.fullName$.asObservable();
  }
  public setFullNameForStore(fullname:string){
    this.fullName$.next(fullname)
  }
  public getUserIdFromStore(){
    return this.id$.asObservable();
   }
   public setUserIdForStore(userId:string){
     this.id$.next(userId)
   }
}
