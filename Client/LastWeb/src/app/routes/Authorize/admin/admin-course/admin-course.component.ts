import { Component, OnInit } from '@angular/core';
import { WordbookManagementService } from 'src/app/services/wordbook-management/wordbook-management.service';

@Component({
  selector: 'app-admin-course',
  templateUrl: './admin-course.component.html',
  styleUrls: ['./admin-course.component.css']
})
export class AdminCourseComponent implements OnInit {
  listCourse : any
  ngOnInit(): void {
    this.fetchData()
  }
  constructor(private http: WordbookManagementService){}
  fetchData(){
    this.http.getAllCourse().subscribe(res => {
      this.listCourse = res
    })
  }

}
