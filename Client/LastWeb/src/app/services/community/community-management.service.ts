import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CommunityManagementService {
  baseUrl: string = 'http://localhost:43268/api/posts/';
  baseComment : string = 'http://localhost:43268/api/comment/';
  constructor(private http : HttpClient) { }
  getAllPost() {
    console.log("okk")
   return this.http.get(this.baseUrl);

  }
  getAllPostID(id : any){
    return this.http.get(this.baseUrl  +id) ;
  }
  getAllCourseByUserId(userId : any){
    return this.http.get(this.baseUrl + userId);
  }
  handelSharePost(userId : any,body : any){
    return this.http.post(this.baseUrl+userId,body)
  }
  deletePost(id :any){
    return this.http.delete(this.baseUrl+id);
  }
  getAllComment(url: any){
    return this.http.get(this.baseComment+url)
  }
  sendComment(userId : any ,postId:any, body : any){
    return this.http.post(this.baseComment+userId+'/'+postId,body)
  }
  handelRating(courseId:any,rating :any){
    return this.http.put(this.baseUrl+courseId+'/'+rating, '');
  }
  handelComment(userId:number,postId : number,content:any){
    return this.http.post(`${this.baseUrl}+ ${userId}+'/'+${postId}`, content)
  }
  importCourse(userId:any,postId:any){
    return this.http.post(this.baseUrl+userId+'/'+postId,'');
  }
}
