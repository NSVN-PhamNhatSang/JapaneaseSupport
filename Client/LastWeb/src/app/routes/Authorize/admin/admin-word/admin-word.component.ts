import { Component } from '@angular/core';
import { WordbookManagementService } from 'src/app/services/wordbook-management/wordbook-management.service';

@Component({
  selector: 'app-admin-word',
  templateUrl: './admin-word.component.html',
  styleUrls: ['./admin-word.component.css']
})
export class AdminWordComponent {
constructor(private http : WordbookManagementService){}
listWord : any ;
ngOnInit():void{
this.fetchData()
}
fetchData(){
this.http.getAllWord().subscribe(res => {
  this.listWord = res
})
}
}
