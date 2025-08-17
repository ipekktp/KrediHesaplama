import { Component, OnInit } from '@angular/core';
import { CommonModule, NgFor, NgIf } from '@angular/common';
import { KrediService } from '../../Services/kredi.service';

// Kredi türlerini backenddeki CreditTypeId ile eşleştirmek için enumı burada da tanımlıyoruz
export enum KrediTuruEnum {
  'İhtiyaç' = 1,
  'Taşıt' = 2,
  'Konut' = 3,
}

// Backendden gelen veri yapısını tanımlayan arayüz 
export interface Kredi {
  basvuruId: number;
  tutar: number;
  vade: number;
  creditTypeId: KrediTuruEnum;
  isCancelled: boolean;
  basvuruTarihi: string;
  userId: number;
  tckn: string;
}

@Component({
  selector: 'app-kredi-liste',
  standalone: true,
  imports: [CommonModule, NgFor, NgIf],
  template: `
    <div class="container">
      <h2>Kredilerim</h2>

      <div *ngIf="krediler.length === 0" class="empty-list">
        Henüz bir kredi başvurunuz bulunmamaktadır.
      </div>

      <div *ngIf="krediler.length > 0">
        <ul class="kredi-list">
          <!-- Backend'den gelen verilere göre kredileri listeliyoruz -->
          <li *ngFor="let kredi of krediler" class="kredi-item">
            <div class="kredi-info">
              <!-- Backendden gelen basvuruIdyi kullanıyoruz -->
              <p><strong>Başvuru ID:</strong> {{ kredi.basvuruId }}</p>
              <!-- Enumı kullanarak sayısal IDyi stringe çeviriyoruz -->
              <p><strong>Kredi Türü:</strong> {{ getKrediTuruAdi(kredi.creditTypeId) }}</p>
              <p><strong>Tutar:</strong> {{ kredi.tutar | number }} TL</p>
              <p><strong>Vade:</strong> {{ kredi.vade }} Ay</p>
              <!-- İptal durumuna göre durumu gösteriyoruz -->
              <p><strong>Başvuru Durumu:</strong> {{ kredi.isCancelled ? 'İptal Edildi' : 'Devam Ediyor' }}</p>
            </div>
            <div class="kredi-actions">
              <!-- Yalnızca iptal edilmemiş başvurular için butonu gösteriyoruz -->
              <button *ngIf="!kredi.isCancelled" (click)="onIptalEt(kredi.basvuruId)" class="btn secondary">İptal Et</button>
            </div>
          </li>
        </ul>
      </div>
    </div>

    <!-- Özel success ve error mesaj kutularımız -->
    <div *ngIf="isSuccess" class="message-box success">
      <p>{{ message }}</p>
      <button (click)="closeMessage()" class="close-btn">&times;</button>
    </div>
    <div *ngIf="isError" class="message-box error">
      <p>{{ message }}</p>
      <button (click)="closeMessage()" class="close-btn">&times;</button>
    </div>
  `,
  styles: [`
    .kredi-list {
      list-style-type: none;
      padding: 0;
      display: flex;
      flex-direction: column;
      gap: 1rem;
    }

    .kredi-item {
      background-color: #fff;
      padding: 1.5rem;
      border-radius: 12px;
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
      display: flex;
      justify-content: space-between;
      align-items: center;
      transition: transform 0.3s ease, box-shadow 0.3s ease;
    }

    .kredi-item:hover {
      transform: translateY(-3px);
      box-shadow: 0 6px 16px rgba(0, 0, 0, 0.1);
    }

    .kredi-info p {
      margin: 0.3rem 0;
      color: #5d4037;
    }

    .kredi-actions {
      display: flex;
      gap: 0.5rem;
    }

    .btn.secondary {
      padding: 0.5rem 1rem;
      font-size: 0.9rem;
      border: 1px solid #c62828;
      background-color: transparent;
      color: #c62828;
      transition: background-color 0.3s, color 0.3s;
    }

    .btn.secondary:hover {
      background-color: #c62828;
      color: #fff;
    }

    .empty-list {
      text-align: center;
      margin-top: 3rem;
      font-size: 1.2rem;
      color: #795548;
    }

    .message-box {
      position: fixed;
      bottom: 20px;
      right: 20px;
      padding: 1rem;
      border-radius: 8px;
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
      display: flex;
      align-items: center;
      justify-content: space-between;
      animation: fadeIn 0.5s ease-out;
    }
    
    .message-box.success {
      background-color: #e8f5e9;
      color: #388e3c;
    }

    .message-box.error {
      background-color: #ffebee;
      color: #d32f2f;
    }

    .close-btn {
      background: none;
      border: none;
      font-size: 1.5rem;
      color: inherit;
      cursor: pointer;
      margin-left: 1rem;
    }

    @keyframes fadeIn {
      from {
        opacity: 0;
        transform: translateY(20px);
      }
      to {
        opacity: 1;
        transform: translateY(0);
      }
    }
  `]
})
export class KrediListeComponent implements OnInit {
  // Arayüzü kullanarak krediler dizisinin tipini belirliyoruz.
  krediler: Kredi[] = [];
  isSuccess: boolean = false;
  isError: boolean = false;
  message: string = '';

  constructor(private krediService: KrediService) {}

  ngOnInit() {
    const tckn = localStorage.getItem('tckn');
    
    if (tckn) {
      // API çağrısını TCKN'ye göre yap
      this.krediService.getBasvurular(tckn).subscribe({
        next: (res) => {
          this.krediler = res;
          // Gelen veriyi konsolda göster, hata ayıklamaya yardımcı olur
          console.log('Kredi listesi yüklendi:', this.krediler);
        },
        error: (err) => {
          console.error('Kredi listesi alınamadı:', err);
          this.isError = true;
          this.message = 'Kredi listenizi alırken bir hata oluştu.';
        }
      });
    }
  }

  // Enum'daki sayısal değeri string karşılığına çeviren yardımcı metot
  // Cast işlemini kaldırarak hatayı gideriyoruz
  getKrediTuruAdi(creditTypeId: number): string {
    // TypeScript'e bu dönüşümün kasıtlı olduğunu belirtmek için basit bir tip kontrolü ekliyoruz
    return KrediTuruEnum[creditTypeId];
  }

  // İptal işlevi
  onIptalEt(basvuruId: number) {
    if (confirm('Bu başvuruyu iptal etmek istediğinizden emin misiniz?')) {
      this.krediService.iptalEt(basvuruId).subscribe({
        next: () => {
          this.isSuccess = true;
          this.message = 'Başvuru başarıyla iptal edildi.';
          
          // Listeyi yerel olarak güncelle
          const iptalEdilenKredi = this.krediler.find(k => k.basvuruId === basvuruId);
          if (iptalEdilenKredi) {
            iptalEdilenKredi.isCancelled = true;
          }
          
        },
        error: (err) => {
          this.isError = true;
          this.message = `İptal sırasında bir hata oluştu: ${err.error?.mesaj || 'Bilinmeyen Hata'}`;
          console.error('İptal hatası:', err);
        }
      });
    }
  }
  
  closeMessage() {
    this.isSuccess = false;
    this.isError = false;
    this.message = '';
  }
}
