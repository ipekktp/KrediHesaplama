import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './components/register/register';
import { KrediListeComponent } from './kredi-liste/kredi-liste.component';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from './auth.guard';
import { BasvuruComponent } from './basvuru/basvuru.component';  
import { KrediComponent } from './hesapla/hesapla.component'; 
import { WelcomePageComponent } from './welcome/welcome.component'; 


export const routes: Routes = [
  { path: '', component: WelcomePageComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
  { path: 'hesapla', component: KrediComponent }, 
  { path: 'basvuru', component: BasvuruComponent }, 
  { path: 'kredi-liste', component: KrediListeComponent, canActivate: [AuthGuard] },
  { path: '**', redirectTo: '' }
];
