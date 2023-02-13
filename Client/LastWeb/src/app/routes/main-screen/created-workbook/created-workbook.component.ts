import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import { NzMessageService } from 'ng-zorro-antd/message';
import { WordManagementService } from 'src/app/services/word-management/word-management.service';
import { WordbookManagementService } from 'src/app/services/wordbook-management/wordbook-management.service';
import { staticPath } from 'src/app/utils/staticPath';
import { AuthService } from '../../Authorize/serviceAuthorize/auth.service';

@Component({
  selector: 'app-created-workbook',
  templateUrl: './created-workbook.component.html',
  styleUrls: ['./created-workbook.component.css'],
})
export class CreatedWorkbookComponent {
  allWordBook: any = [];
  isVisible = false;
  // public Name = new FormControl('');
  public courseForm = new FormGroup({
    category: new FormControl(''),
    level: new FormControl(''),
    name: new FormControl(''),
  });
  Name: any = '';
  Level: any = '';
  Category: any;
  data: any;
  // public Category = new FormControl('');
  // public Level = new FormControl('')
  constructor(
    private router: Router,
    private wordbookService: WordbookManagementService,
    private wordService: WordManagementService,
    private message: NzMessageService,
    private auth: AuthService,
    private toast : NgToastService
  ) {}
  hGutter = 16;
  vGutter = 16;
  count = 3;
  array = new Array(this.count);
  userIdFromToken: any;
  ngOnInit(): void {
    this.userIdFromToken = this.auth.getUserIdFromToken();
    this.fetchData(this.userIdFromToken);
    console.log(this.userIdFromToken);
  }

  async fetchData(id: any) {
    await this.getAllWordBook(id);
  }

  async getAllWordBook(id: any) {
    var response = await this.wordbookService
      .getAllCourseByUserId(id).subscribe({
        next: (res: any) => {
          this.allWordBook = res
          
        },
        error: (err: any) => {
          this.toast.warning({
            detail: 'warning',
            summary: 'Người dùng  không có khóa học nào',
            duration: 5000,
          });
        },
      });
   
    console.log('this.allMatches');
    console.log(this.allWordBook);
    console.log(this.allWordBook[0]);
  }
  async onClickSubmit() {
    if (this.isVisible) {
      this.isVisible = false;
    }
    this.message.loading('Đã thêm khóa học thành công');
    this.Name = '';
    this.Level = '';
    this.Category = '';
    try {
      await this.wordbookService
        .createWordBook(this.userIdFromToken, this.courseForm.value)
        .toPromise();
      } catch (error) {
        console.error(error);
      }
      console.log(this.courseForm);
      this.getAllWordBook(this.userIdFromToken);
  }

  goToDetailWord(id: any): any {
    this.router.navigate([`/${staticPath.WORD_LIST}`, { id }]);
    this.wordbookService.setCurrentWordBook(id);
    // this.saveDataCache(constant.CACHE_WORDBOOK_LABEL, wordBook);
  }
  async deleteCourse(wordBook: any) {
    var id = wordBook.courseId;
    if (confirm('Bạn có chắc sẽ xóa khóa học này không?')) {
      try {
        await this.wordbookService.deleteWordBook(id).toPromise();
      } catch (error) {
        console.error(error);
      }
      this.getAllWordBook(this.userIdFromToken);
    }
  }
  onClickAddCourse() {
    this.isVisible = true;
    console.log(this.isVisible);
  }
  // When the user clicks on <span> (x), close the modal
  closeModal() {
    if (this.isVisible) {
      this.isVisible = false;
    }
  }

  // When the user clicks anywhere outside of the modal, close it
  //  function(t) {
  //   if (event.target == modal) {
  //     modal.style.display = "none";
  //   }
  // }
  saveDataCache(label: string, data: any) {
    localStorage.setItem(label, JSON.stringify(data));
  }
}
