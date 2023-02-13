import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { constant } from 'src/app/utils/constant';
@Injectable({
  providedIn: 'root'
})
export class WordManagementService {

  currentWordBook: any;
  baseUrl: string = 'http://localhost:43268/api/';
  CommunityUrl: string = 'http://localhost:43268/api/posts/';
  userUrl : string = 'http://localhost:43268/api/users'
  private readonly API_ROUTES = {
    getWord: (wordId : string) => `${this.baseUrl}/course/join/${wordId}`
  }
  constructor(private http: HttpClient) {}

  
  // getWordById(id: any) {
  //   return this.http.get(`${this.baseUrl}course/join/${id}`);
  // }
  // setCurrentWordBook(data: any): void {
  //   this.currentWordBook = data;
  //   this.saveDataCache(constant.CACHE_WORDBOOK_LABEL, data);
  // }
getAllUser(){
  return this.http.get(this.userUrl)
}
deleteUser(userId : any){
  return this.http.delete(this.userUrl+'/'+userId)
}
editUser(userId : any,post:any){
  return this.http.put(this.userUrl+'/change/'+userId,post)
}
  updateWordBook(id: any, data: any) {
    return this.http.put(`${this.baseUrl}/${id}`, data);
  }
  createWordBook(data : any) {
    return this.http.post(this.baseUrl,data);
  
  }
  deleteWordBook(id:any){
  return this.http.delete(this.baseUrl);
  }
  setCurrentListWord(data :any){
    this.currentWordBook = data;
    this.saveDataCache(constant.CACHE_WORDBOOK_LABEL, data);
  }
  saveDataCache(label: string, data: any) {
    localStorage.setItem(label, JSON.stringify(data));
  }


 getAllPost() {
    console.log("okk")
   return this.http.get(this.baseUrl);

  }
  getAllPostID(id : any){
    return this.http.get(this.baseUrl  +id) ;
  }
  handelSharePost(){
    
  }
  deletePost(id :any){
    return this.http.delete(this.baseUrl+id);
  }
  getAllComment(url: string){
    return this.http.get(this.baseUrl)
  }
  handelRating(courseId:any,rating :any){
    return this.http.put(this.baseUrl+courseId+'/rating', rating);
  }
  handelComment(userId:number,postId : number,content:any){
    return this.http.post(`${this.baseUrl}+ ${userId}+'/'+${postId}`, content)
  }
}