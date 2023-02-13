import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { NgToastService } from 'ng-angular-popup';
import { NzMessageComponent, NzMessageService } from 'ng-zorro-antd/message';
import { Fragment } from 'react';
import { CommunityManagementService } from 'src/app/services/community/community-management.service';
import { WordManagementService } from 'src/app/services/word-management/word-management.service';
import { WordbookManagementService } from 'src/app/services/wordbook-management/wordbook-management.service';
import { AuthService } from '../../Authorize/serviceAuthorize/auth.service';
interface courseCommunity {
  avatar: string;
  authorName: string;
  titleName: string;
  content: string;
  totalWorld: number;
  headerContent?: string;
  rating: any;
  comment?: string;
}
@Component({
  selector: 'app-community',
  templateUrl: './community.component.html',
  styleUrls: ['./community.component.css'],
})
export class CommunityComponent implements OnInit {
  userIdFromToken: any;
  @ViewChild('comment') inputComment: any;
  commentData: any;
  isListWord : any = false ;
  ngOnInit(): void {
    this.fetchData();
    this.userIdFromToken = this.auth.getUserIdFromToken();
    this.getAllCourseById();
    console.log(this.userIdFromToken);
  }
  constructor(
    private auth: AuthService,
    private toast: NgToastService,
    private message: NzMessageService,
    private http: CommunityManagementService,
    private course : WordbookManagementService
  ) {}
listWord : any ;
  isRating: any;
  isUpload = true;
  isHintPost: any;
  hasExpand = true;
  isVisible = false;
  hasComment: any;
  postForm = new FormGroup({
    postId : new FormControl(''),
    
    postImage: new FormControl(''),
    postContent: new FormControl(''),
    courseId: new FormControl(''),
    courseName : new FormControl(''),
    userName : new FormControl('')
  });
  Content: any;
  Image: any;
  CourseID: any;
  courseImport : any;
  data: any;
  stars: number[] = [1, 2, 3, 4, 5];
  rating: any;
  commentForm = new FormGroup({
    comment: new FormControl(''),
  });
  listPost: any;
  handleOpenRating(idPost: any): any {
    this.isRating = this.isRating === idPost ? null : idPost;
  }
  listCourse: any;
  openModal(): any {
    this.isVisible = true;
  }
  openModalCourse(idCourse : any) :any {
    this.isListWord = true ;
    console.log(idCourse)
   this.course
          .getWordbookById(idCourse)
          .subscribe(res =>{

            
            this.listWord = res;
          }
            )
  }
  closeModalCourse() : any {
    this.isListWord = false ;
  }
  onSubmit(postId: any) {
    this.inputComment.nativeElement.value = '';
    this.http
      .sendComment(this.userIdFromToken, postId, this.commentForm.value)
      .subscribe({
        next: (res: any) => {
          this.hasComment = null;
          this.toast.warning({
            detail: 'warning',
            summary: 'Có chút vấn đề khi comment bài viết này',
            duration: 5000,
          });
        },
        error: (err: any) => {
          console.log(err);
          this.commentForm.reset();
          this.message.info('Đã gửi bình luận thành công');
          console.log(postId);
          this.onClickHandleCommentExpand(postId);
        },
      });
  }
  inputImage(e: any): any {
    this.isUpload = false;
    console.log(this.isUpload);
    if (e.target.files) {
      var reader = new FileReader();
      reader.readAsDataURL(e.target.files[0]);
      reader.onload = (event: any) => {
        this.Image = event.target.result;
        // this.postForm.patchValue({
        //   image: event.target.result
        // })
      };
      // this.postForm.get('image').updateValueAndValidity();
    }
  }
  closeModal(): any {
    if (this.isVisible) {
      this.isVisible = false;
    }
  }
  //Rating bài viết của người dùng
  async countStart(courseId: any, star: any) {
    this.isRating = null;
try{

  await this.http.handelRating(courseId, star).toPromise()
} catch (error) {
  this.toast.warning({
    detail: 'Warning',
    summary: 'Rating chưa được ghi nhận',
    duration: 2000,
  });}
    this.fetchData();
  }

  async onClickSend() {
    this.Image = '';
    this.Content = '';
    this.CourseID = '';
    this.isUpload = true;
    console.log(this.postForm.value);
        try {
      await this.http.handelSharePost(this.userIdFromToken, this.postForm.value).toPromise();
      this.toast.warning({
        detail: 'Warning',
        summary: 'Không thể đăng bài',
        duration: 2000,
      });
      } catch (error) {
        this.toast.info({
          detail: 'Infor',
          summary: 'Đã đăng bài ',
          duration: 2000,
        });
      }
    this.fetchData();
    this.hasExpand = true;
  }
  onClickClose() : void {
    this.hasExpand = true;
  }
  onClickHandleExpand(): void {
    this.hasExpand = false;
  }
  onClickHandleCommentExpand(idPost: any): void {
    this.hasComment = this.hasComment === idPost ? null : idPost;

    this.http.getAllComment(idPost).subscribe({
      next: (res: any) => {
        this.commentData = res;
      },
      error: (err: any) => {
        this.hasComment = null;
        this.toast.info({
          detail: 'Info',
          summary: 'Post không có bình luận nào',
          duration: 2000,
        });
      },
    });
  }
  onClickHandleHint(idPost: any): void {
    if (confirm('Bạn có chắc sẽ ẩn bài viết này đi không ')) {
      this.isHintPost = this.isHintPost === idPost ? null : idPost;
      this.message.info('Đã ẩn bài viết của người này');
    } else {
      this.message.info('Bài viết vẫn sẽ tồn tại trong tường của bạn');
    }
  }
 
  getAllCourseByUserId(): any {}
 async fetchData() {
 
    var response = await this.http.getAllPost().subscribe (
    (res: any) => {
        this.listPost = res
    });
      console.log(this.listPost);
  }
  
  getAllCourseById(): any {
    this.course.getAllCourseByUserId(this.userIdFromToken).subscribe((res) => {
      this.listCourse = res;
    });
  }
  onClickCourseOfPost():any {

  }
  onClickCourse(courseId : any) : any {
    this.closeModal();
    console.log(courseId);
    // this.courseId.setValue() = courseId ;
    this.postForm.controls['courseId'].setValue(courseId);
  }
  editComment(commentId : any ) {
  
  }
   importToMyCourse(idCourse : any){
    this.courseImport = idCourse;
 this.http.importCourse(this.userIdFromToken,idCourse).subscribe({
    next: (res: any) => {
      this.toast.info({
        detail: 'Info',
        summary: 'Đã nhập thêm khóa học này vào kh',
        duration: 2000,
      });
    },
    error: (err: any) => {
      this.toast.warning({
        detail: 'warning',
        summary: 'Khóa học này là của bạn không thể thêm',
        duration: 1000,
      });    },
  });


  }
}
function formatDistance(arg0: Date, arg1: Date) {
  throw new Error('Function not implemented.');
}
