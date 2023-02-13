import { Component, OnInit } from '@angular/core';
import { CommunityManagementService } from 'src/app/services/community/community-management.service';

@Component({
  selector: 'app-admin-post',
  templateUrl: './admin-post.component.html',
  styleUrls: ['./admin-post.component.css']
})
export class AdminPostComponent implements OnInit{
  listPost : any ;
ngOnInit(): void {
  this.fetchData()
}
fetchData(){
  this.community.getAllPost().subscribe(res => {
    this.listPost = res
  })
}
 async deletePost(postid : any ){
  if(confirm('Bạn có chắc sẽ xóa bài viết này không ')){

    await this.community.deletePost(postid).toPromise()
  }
  this.fetchData()
}
constructor(private community : CommunityManagementService){}
}
