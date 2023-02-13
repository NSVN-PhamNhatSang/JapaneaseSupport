import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, retry } from 'rxjs';
import { constant } from 'src/app/utils/constant';

@Injectable({
  providedIn: 'root'
})
export class WordbookManagementService {


  currentWordBook: any;
  // baseUrl: string = 'https://632d24b2519d17fb53b8943d.mockapi.io/word-books';
  baseUrl: string = 'http://localhost:43268/api/course/';
  wordUrl: string = 'http://localhost:43268/api/word/'
  constructor(private http: HttpClient) {}
  getAllCourse(){
    return this.http.get(this.baseUrl)
  }
getAllWord(){
  return this.http.get(this.wordUrl);
}
  getAllWordbook() {
    return this.http.get(this.baseUrl);
  }
  getAllCourseByUserId(id:any){
    return this.http.get(`${this.baseUrl}${id}`)
  }
  createWordByCourseId(idPost:any,data:any): Observable<any>{
    return this.http.post(`${this.baseUrl}create/${idPost}`,data)
  }
  getWordbookById(id: any) {
    return this.http.get(`${this.baseUrl}join/${id}`);
  }

  updateWordBook(id: any, data: any) {
    return this.http.put(`${this.baseUrl}${id}`, data);
  }
  createWordBook(id:any,data : any) {
    return this.http.post(`${this.baseUrl}${id}`,data);
  
  }
  deleteWordBook(id:any):Observable<any>{
  return this.http.delete(`${this.baseUrl}${id}`);

  }
  setCurrentWordBook(data: any): void {
    this.currentWordBook = data;
    console.log(this.currentWordBook)
    this.saveDataCache(constant.CACHE_WORDBOOK_LABEL, data);
  }
  saveDataCache(label: string, data: any) {
    localStorage.setItem(label, JSON.stringify(data));
  }
}

