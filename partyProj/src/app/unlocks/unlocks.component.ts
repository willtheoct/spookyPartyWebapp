import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppComponent } from '../app.component';
import { timer } from 'rxjs/observable/timer';

@Component({
  selector: 'app-unlocks',
  templateUrl: './unlocks.component.html',
  styleUrls: ['./unlocks.component.css']
})
export class UnlocksComponent implements OnInit {

  constructor(private http: HttpClient) { }

  ngOnInit() {
  }
  unlockResult = "";
  unlockResultWait = 0;
  unlockCode = "";
  tryUnlock() {
    this.http.get<string>(AppComponent.hostServer + "/unlock?userId=" + AppComponent.userId + "?code=" + this.unlockCode).subscribe(x => {
      this.unlockResultWait++;
      this.unlockResult = x;
      timer(5000).subscribe(() => {
        this.unlockResultWait--;
        this.unlockResult = "";
      });
    });
  }

}
