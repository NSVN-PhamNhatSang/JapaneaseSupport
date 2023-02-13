import { Component, OnInit } from '@angular/core';
import { async } from '@angular/core/testing';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import { NzMessageService } from 'ng-zorro-antd/message';
import { WordManagementService } from 'src/app/services/word-management/word-management.service';
import { WordbookManagementService } from 'src/app/services/wordbook-management/wordbook-management.service';
import { constant } from 'src/app/utils/constant';

@Component({
  selector: 'app-word-list',
  templateUrl: './word-list.component.html',
  styleUrls: ['./word-list.component.css'],
})
export class WordListComponent implements OnInit {
  currentWordBook: any;
  cachedWordBook: any;
  isVisible = false;
  currentData: any;
  currentWord: any;
  isAdd: any = 1;
  public wordForm = new FormGroup({
    wordHiragana: new FormControl(''),
    wordMeaning: new FormControl(''),
    wordKanji: new FormControl(''),
    example: new FormControl(''),
    wordStaus: new FormControl(''),
  });
  Hiragana = '';
  Kanji = '';
  Meaning = '';
  Example = '';
  constructor(
    private wordbookService: WordbookManagementService,
    private message: NzMessageService,
    private toast : NgToastService
  ) {}
  ngOnInit(): void {
    this.fetchData();
    this.currentWord = this.currentWordBook[0];
  }
  // xem trước review qua flash card
  onClickList(data: any) {
    this.currentWord = data;
    console.log(this.currentData);
  }
  async onClickAddWord() {
    console.log(this.wordForm);
    this.isVisible = false;
    this.message.loading('Đang thêm từ mới vào course');
    this.Hiragana = '';
    this.Kanji = '';
    this.Meaning = '';
    this.Example = '';

    try {
      const response = await this.wordbookService
        .createWordByCourseId(
          JSON.parse(this.cachedWordBook),
          this.wordForm.value
        ).toPromise();

      console.log(response);
    } catch (error) {
      console.error(error);
    }
    await this.fetchData();
  }

  //auto lay bang id okkk

  async fetchData() {
    this.cachedWordBook = localStorage.getItem(constant.CACHE_WORDBOOK_LABEL);
    if (this.cachedWordBook) {
      this.currentWordBook = JSON.parse(this.cachedWordBook);
      console.log(this.currentWordBook);
      try {
        const response = await this.wordbookService
          .getWordbookById(this.currentWordBook)
          .toPromise();
        this.currentData = response;
      } catch (error) {
        this.toast.warning({
          detail: 'warning',
          summary: 'Bài viết không có từ nào,hãy thêm vào',
          duration: 5000,
        });
        console.error(error);
      }
    }
  }

  async onClickDeleteWord(id: any) {
    console.log(id);
    if (confirm('Are your sure to delete word?')) {
      try {
        await this.wordbookService.deleteWordBook(id).toPromise();
        this.message.info('Đã xóa từ khỏi danh sách này');
      } catch (error) {
        console.error(error);
      }
      this.fetchData();
    }
  }

  openModal(): any {
    this.isVisible = true;
  }
  closeModal(): any {
    if (this.isVisible) {
      this.isVisible = false;
    }
  }
}
