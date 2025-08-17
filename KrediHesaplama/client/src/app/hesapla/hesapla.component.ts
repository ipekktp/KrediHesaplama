import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule, NgIf, CurrencyPipe } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { KrediService } from '../../Services/kredi.service';

// Kredi türlerini backend'deki CreditTypeId ile eşleştirmek için enum
export enum KrediTuruEnum {
  İhtiyaç = 1,
  Taşıt = 2,
  Konut = 3,
}

@Component({
  selector: 'app-kredi',
  templateUrl: './hesapla.component.html',
  styleUrls: ['./hesapla.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule, NgIf, CurrencyPipe, HttpClientModule]
})
export class KrediComponent implements OnInit {
  krediTutar: number = 50000;
  vade: number = 12;
  aylikTaksit: number = 0;
  toplamOdenecekTutar: number = 0;
  gosterSonuc: boolean = false;
  seciliKrediTuru: 'İhtiyaç' | 'Taşıt' | 'Konut' = 'İhtiyaç';
  
  faizOranlari = {
    'Konut': 2.99,
    'Taşıt': 3.99,
    'İhtiyaç': 4.99,
  }; 
    
  showPaymentPlan: boolean = false;
  paymentPlan: any[] = [];

  minTutar: number = 0;
  maxTutar: number = 0;
  minVade: number = 0;
  maxVade: number = 0;

    
  constructor(private krediService: KrediService) { }

  ngOnInit(): void {
    this.guncelleKrediBilgilerini();
  }

  onKrediTuruDegisti(tur: 'İhtiyaç' | 'Taşıt' | 'Konut') {
    this.seciliKrediTuru = tur; 
    this.gosterSonuc = false;
    this.showPaymentPlan = false;
    this.guncelleKrediBilgilerini();
  }
  
  
  guncelleKrediBilgilerini() {
    switch (this.seciliKrediTuru) {
      case 'İhtiyaç':
        this.minTutar = 10000;  
        this.maxTutar = 100000; 
        this.minVade = 3;       
        this.maxVade = 36;     
        break;
      case 'Taşıt':
        this.minTutar = 100000;   
        this.maxTutar = 1000000;  
        this.minVade = 3;         
        this.maxVade = 60;        
        break;
      case 'Konut':
        this.minTutar = 300000;   
        this.maxTutar = 4000000;  
        this.minVade = 3;         
        this.maxVade = 120;       
        break;
      default:
        this.minTutar = 0;
        this.maxTutar = 0;
        this.minVade = 0;
        this.maxVade = 0;
        break;
    }
    
    // Geçerli kredi tutar ve vade değerlerinin yeni aralık içinde kalmasını sağlar
    if (this.krediTutar < this.minTutar) this.krediTutar = this.minTutar;
    if (this.krediTutar > this.maxTutar) this.krediTutar = this.maxTutar;
    if (this.vade < this.minVade) this.vade = this.minVade;
    if (this.vade > this.maxVade) this.vade = this.maxVade;
  }

hesapla() {
    const krediTuruId = KrediTuruEnum[this.seciliKrediTuru];
    
    // Backend'e gönderilecek veri nesnesini hazırlıyoruz.
    // Özellik adlarının KrediService'in beklediği gibi camelCase (küçük harfle başlayan) olduğundan emin olun.
    const hesaplamaData = {
      creditTypeId: krediTuruId,
      tutar: this.krediTutar,
      vade: this.vade
    };
    
    this.krediService.hesapla(hesaplamaData).subscribe({
      next: (response) => {
        this.aylikTaksit = response.aylikTaksit;
        this.toplamOdenecekTutar = response.toplamOdeme; 
        this.gosterSonuc = true;
        this.showPaymentPlan = false; 
      },
      error: (error) => {
        console.error('Hesaplama sırasında bir hata oluştu:', error);
        alert('Hesaplama sırasında bir hata oluştu. Lütfen tekrar deneyin.');
        this.gosterSonuc = false;
      }
    });
  }
  

  getPaymentPlan() {
    this.showPaymentPlan = !this.showPaymentPlan;
    
    if (this.showPaymentPlan) {
      this.paymentPlan = [];
      const yillikFaiz = this.faizOranlari[this.seciliKrediTuru] / 100;
      const aylikFaizOrani = yillikFaiz / 12;
      let kalanBorc = this.krediTutar;

      for (let ay = 1; ay <= this.vade; ay++) {
        const faizOdeme = kalanBorc * aylikFaizOrani;
        const anaparaOdeme = this.aylikTaksit - faizOdeme;
        kalanBorc -= anaparaOdeme;

        this.paymentPlan.push({
          ay: ay,
          taksitTutari: this.aylikTaksit,
          anapara: anaparaOdeme,
          faiz: faizOdeme,
          kalanBorc: kalanBorc > 0 ? kalanBorc : 0,
        });
      }
    }
  }
}
