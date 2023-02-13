import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../Authorize/serviceAuthorize/api.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit {
  public users:any = [];
  public listRankUsers = [];
  listMedal : any = [
    {
      rank : 1 ,
      image : "https://cdn-icons-png.flaticon.com/512/1910/1910476.png"

    },
    {
      rank : 2 ,
      image : "https://cdn-icons-png.flaticon.com/512/1910/1910479.png  "

    },
    {
      rank : 3,
      image : "https://cdn-icons-png.flaticon.com/512/2583/2583434.png"

    }
  ]
  listRankUser: any = [
    {
      rank: 1,
      avatar: 'https://i.postimg.cc/Z5wQKJmh/fchunters.png',
      name: 'Lê Đức Huy',
      word: 120,
    },
    {
      rank: 2,
      avatar: 'https://i.postimg.cc/Z5wQKJmh/fchunters.png',
      name: 'Phạm Nhật Huy',
      word: 110,
    },
    {
      rank: 3,
      avatar: 'https://i.postimg.cc/Z5wQKJmh/fchunters.png',
      name: 'Tạ Huy',
      word: 100,
    },
    {
      rank: 4,
      avatar: 'https://i.postimg.cc/Z5wQKJmh/fchunters.png',
      name: 'Hoàng Việt ',
      word: 10,
    },
    {
      rank: 5,
      avatar: 'https://i.postimg.cc/Z5wQKJmh/fchunters.png',
      name: ' Việt Dũng',
      word: 10,
    },
    {
      rank: 6,
      avatar: 'https://i.postimg.cc/Z5wQKJmh/fchunters.png',
      name: 'Nhật Sang',
      word: 10,
    },
    {
      rank: 7,
      avatar: 'https://i.postimg.cc/Z5wQKJmh/fchunters.png',
      name: 'Hoàng Việt Huy',
      word: 10,
    },
    {
      rank: 8,
      avatar: 'https://i.postimg.cc/Z5wQKJmh/fchunters.png',
      name: 'Hoàng Việt fw',
      word: 10,
    },
    {
      rank: 8,
      avatar: 'https://i.postimg.cc/Z5wQKJmh/fchunters.png',
      name: 'Hoàng Việt fw',
      word: 10,
    },
    {
      rank: 8,
      avatar: 'https://i.postimg.cc/Z5wQKJmh/fchunters.png',
      name: 'Hoàng Việt fw',
      word: 10,
    },
  
  ];

  listAchievement: any = [
    {
      imageUrl: 'https://cdn-icons-png.flaticon.com/512/5455/5455914.png',
      name: 'Chuỗi 3 ngày',
      date: '12/09/2022',
    },
    {
      imageUrl: 'https://cdn-icons-png.flaticon.com/512/1835/1835046.png',
      name: 'Chuỗi 5 ngày',
      date: '14/09/2022',
    },
    {
      imageUrl: 'https://cdn-icons-png.flaticon.com/512/3021/3021600.png',
      name: 'Chuỗi 7 ngày',
      date: '16/09/2022',
    },
    {
      imageUrl: 'https://cdn-icons-png.flaticon.com/512/1834/1834988.png',
      name: 'Chuỗi 10 ngày',
      date: '19/09/2022',
    },
  ];

  screenHeight: number = 0;

  constructor(private api : ApiService) {}

  ngOnInit(): void {
    this.screenHeight = window.innerHeight;
    this.api.getUsers().subscribe((res :any) => {
      this.listRankUsers = res ;})
      console.log(this.listRankUser)
  }
}
