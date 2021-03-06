import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { DuelComponent } from './duel/duel.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { InventoryComponent } from './inventory/inventory.component';
import { TradeComponent } from './trade/trade.component';
import { UnlocksComponent } from './unlocks/unlocks.component';
import { AchievementsComponent } from './achievements/achievements.component';

const routes: Routes = [
  { path: 'duel', component: DuelComponent },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'inventory', component: InventoryComponent },
  { path: 'trade', component: TradeComponent },
  { path: 'enterCode', component: UnlocksComponent },
  { path: 'achievements', component: AchievementsComponent },
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
];
@NgModule({
  imports: [
    CommonModule,
    RouterModule.forRoot(routes)
  ],
  declarations: [],
  exports: [RouterModule
  ]
})
export class AppRoutingModule { }
