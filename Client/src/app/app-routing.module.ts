import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ChatComponent } from './component/chat/chat.component';
import { ContactComponent } from './component/contact/contact.component';
import { GameComponent } from './component/game/game.component';
import { LoginComponent } from './component/login/login.component';

const routes: Routes = [
  {path: '', component: LoginComponent},
  {path: 'contact', component: ContactComponent },
  {path: 'chat', component: ChatComponent},
  {path: 'game', component: GameComponent}


];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
