import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PartyGoer, playerIds } from '../../models/partyGoer.model';
import { Observable } from 'rxjs/Observable';
import { interval } from 'rxjs/observable/interval';
import { HttpClient } from '@angular/common/http';
import { AppComponent } from '../app.component';
import { isNullOrUndefined } from 'util';
import { currencies } from '../../models/currency.model';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  static notifications: notification[] = [];
  notifications: notification[] = [];
  playerId: number = AppComponent.userId;
  tnl = "";
  thisPlayer: PartyGoer;

  constructor(private router: Router, private http: HttpClient) {
  }

  ngOnInit() {
    interval(5000).subscribe(() => {
      if (AppComponent.loggedIn) {
        this.http.get<notification[]>(AppComponent.hostServer + "notifications?user=" + AppComponent.userId || "").subscribe(x => {
          if (isNullOrUndefined(x)) x = [];
          x = x.reverse().slice(0, 10);
          DashboardComponent.notifications = x;
          this.notifications = x;
          this.thisPlayer = PartyGoer.onlinePlayers.find(x => x.id === AppComponent.userId);
          this.tnl = (this.thisPlayer.level * this.thisPlayer.level + 3) + " Gold, 1 Dubloon";
          if (this.thisPlayer.inventory[currencies.crystals] > 0) this.tnl = "1 Crystal";
        });
      }
    });
    this.notifications = DashboardComponent.notifications;
    this.playerId = AppComponent.userId;
    console.log(this.playerId);
    if (PartyGoer.onlinePlayers.length > 0) {

      this.thisPlayer = PartyGoer.onlinePlayers.find(x => x.id === AppComponent.userId);
      this.tnl = (this.thisPlayer.level * this.thisPlayer.level + 3) + " Gold, 1 Dubloon";
      if (this.thisPlayer.inventory[currencies.crystals] > 0) this.tnl = "1 Crystal";
    }
  }



  duel() {
    this.router.navigate(["/duel"]);
  }
  trade() {
    this.router.navigate(["/trade"]);
  }
  craft() {
    this.router.navigate(["/inventory"]);
  }
  enterCode() {
    this.router.navigate(["/enterCode"]);
  }
  goToAchievements() {
    this.router.navigate(["/achievements"]);
  }
  levelUp() {
    let thisPlayer = PartyGoer.onlinePlayers.find(x => x.id === AppComponent.userId);
    if (thisPlayer.inventory[currencies.gold] < thisPlayer.level * thisPlayer.level + 3 && thisPlayer.inventory[currencies.crystals] < 1) {
      return;
    }
    this.http.get<PartyGoer>(AppComponent.hostServer + "levelUp?userId=" + AppComponent.userId).subscribe((x) => {
      thisPlayer.inventory = x.inventory;
      thisPlayer.level = x.level;
    });
  }
}

export class notification {
  constructor(public text: string = "") { }
}