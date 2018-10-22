import { Component, OnInit, Input } from '@angular/core';
import { AppComponent } from '../app.component';
import { playerIds } from '../../models/partyGoer.model';

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
  }

  level = 12;
  name = "Will";


}
