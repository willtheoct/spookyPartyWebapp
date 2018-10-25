import { Component, OnInit, Input } from '@angular/core';
import { AppComponent } from '../app.component';
import { playerIds, PartyGoer } from '../../models/partyGoer.model';

@Component({
  selector: 'app-stats',
  templateUrl: './stats.component.html',
  styleUrls: ['./stats.component.css']
})
export class StatsComponent implements OnInit {

  @Input()
  playerId: playerIds;

  constructor() { }

  ngOnInit() {
    this.name = PartyGoer.onlinePlayers[AppComponent.userId].characterName;
  }

  level = 12;
  name = "Will";


}
