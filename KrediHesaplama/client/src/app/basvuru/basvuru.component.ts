import { Component, OnInit } from '@angular/core';
import { CommonModule, CurrencyPipe, NgFor, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { KrediService } from '../../Services/kredi.service';
import { KrediTuruEnum } from '../kredi-liste/kredi-liste.component';

@Component({
  selector: 'app-basvuru',
  standalone: true,
  imports: [CommonModule, NgIf, FormsModule, CurrencyPipe, HttpClientModule],
  templateUrl: './basvuru.component.html',
  styleUrls: ['./basvuru.component.css']
})
export class BasvuruComponent implements OnInit {
  krediTurleri: ('İhtiyaç' | 'Taşıt' | 'Konut')[] = ['İhtiyaç', 'Taşıt', 'Konut'];
  seciliKrediTuru: 'İhtiyaç' | 'Taşıt' | 'Konut' = 'İhtiyaç';

  krediTutar: number = 50000;
  minTutar: number = 0;
  maxTutar: number = 0;

  minVade: number = 0;
  maxVade: number = 0;
  seciliVade: number = 0;

  aylikTaksit: number = 0;
  toplamOdenecekTutar: number = 0;
  yillikFaizOrani: number = 0;

  isSuccess: boolean = false;
  isError: boolean = false;
  message: string = '';

  constructor(private krediService: KrediService) { }

  ngOnInit(): void {
    // Sayfa yüklendiğinde varsayılan değerleri ayarla
    this.guncelleKrediBilgilerini();
  }

  onKrediTuruDegisti(tur: 'İhtiyaç' | 'Taşıt' | 'Konut') {
    this.seciliKrediTuru = tur;
    this.guncelleKrediBilgilerini();
  }

  onInputChanged() {
    this.hesapla();
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
    }
    
    // Geçerli kredi tutar ve vade değerlerinin yeni aralık içinde kalmasını sağlar
    if (this.krediTutar < this.minTutar) this.krediTutar = this.minTutar;
    if (this.krediTutar > this.maxTutar) this.krediTutar = this.maxTutar;
    if (this.seciliVade < this.minVade || this.seciliVade > this.maxVade) {
      this.seciliVade = this.minVade;
    }
    
    this.hesapla();
  }

  hesapla() {
    const faizOranlari = {
      'Konut': 2.99 / 100,
      'Taşıt': 3.99 / 100,
      'İhtiyaç': 4.99 / 100,
    };
    
    this.yillikFaizOrani = faizOranlari[this.seciliKrediTuru];

    // Aylık faiz oranı hesaplaması
    const aylikFaizOrani = this.yillikFaizOrani / 12;

    if (this.krediTutar > 0 && this.seciliVade > 0 && aylikFaizOrani > 0) {
      const pay = aylikFaizOrani * Math.pow((1 + aylikFaizOrani), this.seciliVade);
      const payda = Math.pow((1 + aylikFaizOrani), this.seciliVade) - 1;
      this.aylikTaksit = (this.krediTutar * pay) / payda;
      this.toplamOdenecekTutar = this.aylikTaksit * this.seciliVade;
    } else {
      this.aylikTaksit = 0;
      this.toplamOdenecekTutar = 0;
    }
  }

  basvuruYap() {
    const creditTypeId = KrediTuruEnum[this.seciliKrediTuru];
    const tckn = localStorage.getItem('tckn');
    
    if (!tckn) {
      this.isError = true;
      this.message = 'Başvuru yapmak için lütfen giriş yapın.';
      return;
    }

    const basvuruData = {
      tckn: tckn,
      creditTypeId: creditTypeId,
      tutar: this.krediTutar,
      vade: this.seciliVade,
    };
    
    this.krediService.basvuruYap(basvuruData).subscribe({
      next: (response) => {
        this.isSuccess = true;
        this.message = response.mesaj;
        console.log('Başvuru yapıldı:', response);
      },
      error: (err) => {
        this.isError = true;
        this.message = `Başvuru sırasında bir hata oluştu: ${err.error?.mesaj || 'Bilinmeyen Hata'}`;
        console.error('Başvuru hatası:', err);
      }
    });
  }

  closeMessage() {
    this.isSuccess = false;
    this.isError = false;
    this.message = '';
  }
}
