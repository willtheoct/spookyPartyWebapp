import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PartyGoer, playerIds } from '../../models/partyGoer.model';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  notifications = [];

  constructor(private router: Router) {
  }

  ngOnInit() {
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
