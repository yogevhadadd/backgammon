import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ChatComponent } from './component/chat/chat.component';
import { ContactComponent } from './component/contact/contact.component';
import { GameComponent } from './component/game/game.component';
import { LoginComponent } from './component/login/login.component';
import { SigninComponent } from './component/signin/signin.component';
import { AuthGuard } from './guards/auth.guard';

const routes: Routes = [
  {path: '', component: LoginComponent},
  {path: 'contact', component: ContactComponent },
  {path: 'register', component: SigninComponent},
  {path: 'chat', component: ChatComponent},
  {path: 'game', component: GameComponent}


];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
