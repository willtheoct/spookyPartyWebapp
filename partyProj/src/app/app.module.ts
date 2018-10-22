import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppRoutingModule } from './app-routing.module';
import { HTTP_INTERCEPTORS, HttpClient, HttpClientModule } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


import { AppComponent } from './app.component';
import { DuelComponent } from './duel/duel.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { PlayerlistComponent } from './playerlist/playerlist.component';
import { InventoryComponent } from './inventory/inventory.component';
import { StatsComponent } from './stats/stats.component';
import { TradeComponent } from './trade/trade.component';


@NgModule({
  declarations: [
    AppComponent,
    DuelComponent,
    DashboardComponent,
    PlayerlistComponent,
    InventoryComponent,
    StatsComponent,
    TradeComponent
  ],
  imports: [
    BrowserModule,
    NgbModule.forRoot(),
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
  exports: [RouterModule]
})
export class AppModule { }
