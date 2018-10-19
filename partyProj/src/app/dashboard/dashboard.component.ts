import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PartyGoer, playerIds } from '../../models/partyGoer.model';
import { Observable } from 'rxjs/Observable';
import { interval } from 'rxjs/observable/interval';
import { HttpClient } from '@angular/common/http';
import { AppComponent } from '../app.component';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  notifications: notification[] = [];

  constructor(private router: Router, private http: HttpClient) {
  }

  ngOnInit() {
    this.notifications.push(new notification("test"))
    interval(2000).subscribe(() => {

      this.http.get<notification[]>("notifications?user=" + AppComponent.userId).subscribe(x => this.notifications = x);
    });
  }



  duel() {
    this.router.navigate(["/duel"]);
  }
  trade() {

  }
  craft() {
    this.router.navigate(["/inventory"]);
  }
}

export class notification {
  constructor(public text: string = "") { }
}