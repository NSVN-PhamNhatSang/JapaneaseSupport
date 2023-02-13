import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-word-list-detail',
  templateUrl: './word-list-detail.component.html',
  styleUrls: ['./word-list-detail.component.css']
})
export class WordListDetailComponent {
  @Input() dataClick : any ;
  thisData: any;
  constructor() { }

  ngOnInit(): void {
   console.log(this.dataClick)
  }
  
}
