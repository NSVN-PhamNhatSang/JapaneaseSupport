import { Component } from '@angular/core';
import { WordbookManagementService } from 'src/app/services/wordbook-management/wordbook-management.service';
import { constant } from 'src/app/utils/constant';

@Component({
  selector: 'app-word-detail',
  templateUrl: './word-detail.component.html',
  styleUrls: ['./word-detail.component.css']
})
export class WordDetailComponent {
  currentWordBook: any;
  editingWordBook: any;
  currentWordIndex: number = 0;
  listSortedWord: any[] = [];
  hideResultMode: boolean = true;
  autoReview: boolean = false;
  isShowResult: boolean = false;
  date: any = new Date().toISOString();
  constructor(private wordbookService: WordbookManagementService) {}

  ngOnInit(): void {
    this.fetchData();
  }

  async fetchData() {
    var cachedWordBook = localStorage.getItem(constant.CACHE_WORDBOOK_LABEL);
    console.log(cachedWordBook)
    if (cachedWordBook) {
      this.currentWordBook = JSON.parse(cachedWordBook);
        var response = await this.wordbookService
        .getWordbookById(this.currentWordBook)
        .toPromise();
        this.currentWordBook = response;
    } 
    
    this.listSortedWord = Object.assign([], this.currentWordBook);
    console.log(this.listSortedWord);
  }

  getListSorted() {
    this.currentWordBook.wordList.forEach((element: any) => {
      console.log(element.lastLearnAt);
      console.log(this.date);
      if (element.lastLearnAt > this.date) {
        console.log('lastLearnAt lon hon');
      } else {
        console.log('today lon hon');
      }
    });
  }


  async onClickReaction(wordId: any, level: number) {
    console.log(this.editingWordBook);

    var thisWord = this.listSortedWord.findIndex(
      (element: any) => element.id == wordId
    );
    console.log(thisWord);
    this.currentWordIndex++;
    this.isShowResult = false;
  }

  onClickNext() {
    this.currentWordIndex++;
  }

  onClickShowResult() {
    if (this.isShowResult) {
      this.isShowResult = false;
    } else {
      this.isShowResult = true;
    }
  }
}
