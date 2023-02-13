import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { WordManagementService } from 'src/app/services/word-management/word-management.service';
import { WordbookManagementService } from 'src/app/services/wordbook-management/wordbook-management.service';
import { staticPath } from 'src/app/utils/staticPath';

@Component({
  selector: 'app-my-wordbooks',
  templateUrl: './my-wordbooks.component.html',
  styleUrls: ['./my-wordbooks.component.css']
})
export class MyWordbooksComponent {
  allWordBook: any = [];
  dataCourse : any = [];
    constructor(
      private router: Router,
      private wordbookService: WordbookManagementService ,
      private http : WordManagementService
    ) {}
    totalWordX1 : any ;
    totalWordX2 : any ;
    totalWordX3 :any ;
    totalWordX4 :any ;
    hGutter = 16;
    vGutter = 16;
    count = 3;
    array = new Array(this.count);
   
    ngOnInit(): void {
      this.fetchData();
      this.getListMyCourse();
    }
  
    async fetchData() {
      await this.getAllWordBook();
    }
    getListMyCourse (){
      this.wordbookService.getWordbookById("courseX1").subscribe(res => {
      const X1: any = res ;
      this.totalWordX1 = X1.length ; 
        
      });
      this.wordbookService.getWordbookById("courseX2").subscribe(res => {
        const X1: any = res ;
        this.totalWordX2 = X1.length ; 
          
        });
        this.wordbookService.getWordbookById("courseX3").subscribe(res => {
          const X1: any = res ;
          this.totalWordX3 = X1.length ; 
            
          });
          this.wordbookService.getWordbookById("courseX4").subscribe(res => {
            const X1: any = res ;
            this.totalWordX4 = X1.length ; 
              
            });
                  
  
  
    }
    async getAllWordBook() {
      try {
        const response = await this.wordbookService.getAllWordbook().toPromise();
        this.allWordBook = response;
        console.log('this.allMatches');
        console.log(this.allWordBook);
        console.log(this.allWordBook[0]);
      } catch (error) {
        console.error(error);
      }
    }
    
    goToDetailWord(id: any): any {
      this.router.navigate([`/${staticPath.WORD_LIST}`, { id }]);
      this.wordbookService.setCurrentWordBook(id);
    }
  
    saveDataCache(label: string, data: any) {
      localStorage.setItem(label, JSON.stringify(data));
    }
}
