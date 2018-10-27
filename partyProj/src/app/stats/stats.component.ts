import { Component, OnInit, Input } from '@angular/core';
import { AppComponent } from '../app.component';
import { playerIds, PartyGoer } from '../../models/partyGoer.model';
import { HttpClient } from '@angular/common/http';
import { interval } from 'rxjs/observable/interval';

@Component({
  selector: 'app-stats',
  templateUrl: './stats.component.html',
  styleUrls: ['./stats.component.css']
})
export class StatsComponent implements OnInit {

  @Input()
  playerId: playerIds;
  thisPlayer: PartyGoer;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getStuff();
    interval(10000).subscribe(() => this.getStuff());
    this.name = PartyGoer.onlinePlayers[AppComponent.userId].characterName;
    this.level = PartyGoer.onlinePlayers[AppComponent.userId].level;
    this.thisPlayer = PartyGoer.onlinePlayers[AppComponent.userId];
  }
  getStuff() {
    this.http.get<PartyGoer[]>(AppComponent.hostServer + "players").subscribe(x => {
      this.name = x[AppComponent.userId].characterName;
      this.level = x[AppComponent.userId].level;
      this.thisPlayer = x[AppComponent.userId];
    });
  }

  level = 1;
  name = "connecting...";


}
