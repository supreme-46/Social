import { HttpClient } from '@angular/common/http';
import { Component,OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
 
  constructor(private http: HttpClient){}


  ngOnInit(): void {
    this.http.get('https://localhost:5000/api/users').subscribe({
      next: reponse => this.users = reponse,
      error: error => console.log(error),
      complete: () => console.log('Request Complete!',this.users)

  });
    
  }

  title = 'Social App';
  users:any;

}
