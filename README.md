# OnlineQuizSystem
## Projenin Amacı
İnsanları sınava tabi tutarak yeterliliklerini ölçmek. Bunu yaparken de ne kadar çok soru çözdüğü ile birlikte bu soruları ne kadar sürede çözdüğü de göz önüne alınır. Projede iki türlü puan hesaplama formülü bulunmaktadır:

1- Puan, netin karesinin geçen zamana bölümü ile hesaplanır. net^2/süre (Static)

2- Standart sapma da göz önüne alınarak 1. maddedeki formül ile puan hesaplanır. Dolayısıyla bir soruyu çözdüğünüzde o soru ne kadar az çözülmüşse o kadar çok puan alırsınız. Soruyu çözemediğinizde ise o soru ne kadar çok çözülmüşse o kadar çok puan kaybedersiniz. (Relative)




## Proje teknik özellikleri:

Proje .Net Core MVC mimarisinde geliştirilmiştir. 
Veritabanı olarak Mysql Kullanılmıştır.

Şu kütüphaneler projede aktif olarak kullanılmıştır:

1- Entity Framework Core

2- JQuery

3- JQuery Validate & JQuery Validate Unobtrusive

4- Jquery ile AJAX işlemleri

4- Bootstrap

5- Bootstrap Sweetalert




## Projede kullanılan yapılanmalar:

1- Server tabanlı ModelMetaDataType ile validasyon

2- Client tabanlı otomatik validasyon işlemleri(JQuery Validate & JQuery Validate Unobtrusive ile)

3- Bir kaç Extension metod

4- Sessionları verimli kullanabilmek amacıyla Session Wrapper tasarım deseni.

5- Global hata yönetimi ve global hata loglama sistemi için Exception Filter

6- Dependency Injection

7- Sınav durumunu ve güvenliğini kontrol altında tutmak amacıyla bir çok Action Filter

8- Admin paneline erişebilmek için Claim tabanlı kimlik doğrulama yöntemi.

9- Stored Procedure'lere bağlanmış Entityler. Bu sayede gelişmekte olan Relative puan hesaplama algoritmasının projeyi yeniden derleme ihtiyacı ortadan kaldırılmıştır.



## Projede gelişim sürecine katkıda bulunan yapılanmalar:

1- Yöneticiler ile irtibata geçebilmeleri için Bize Ulaşın formu.

2- Eğer projede beklenmedik bir Exception meydana gelirse sistem bu hatayı veri tabanına kaydeder ve ardından kullanıcıyı Bize Ulaşın sayfasına yönlendirir. Bu yönlendirme sonucunda beklenmedik bir hatayla karşılaştığı kullanıcıya bildirilir ve yaşadıklarını bize iletmesi istenir. Bu sayede projenin gelişimi desteklenmektedir.
 
