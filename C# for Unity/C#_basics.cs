using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{


    //? dosyayı kaydettikten sonra unity'ye geçtiğinde bir 5-6 saniye bekle unity senin kodunu oyuna entegre edebilsin

    



    // Start is called before the first frame update
    void Start()
    {
        

        //? ################### integer;
        // print(21235871256701);    //* sonlarına ";" koyman gerekiyor zorunlu
                    //* bunun direk console'da yazdırılmamasının nedeni bu imageyi hehangi bir game objesine eklemedik
                    //* C# scriptini eklemek için C# dosyamızın üzerine geliyoruz ve basılı tutup bunu istediğmiz objenin add compenents kısmına bırakıyoruz



        //? ################### string;
        // print("Mahmut Esat Arslan"+ "Hello World...");   //* string ifadeleri "" ile kullanıyoruz


        //? ################### float; (ondalıklı sayı)
        // print(3.14);    //* ondalıklı sayıları "." ile ifade ediyoruz  "," ile değil yoksa error verir



        //? ################### değişken oluşturma  // değişken oluştururken mutlaka onun cinsini belirtmek zorundayız bilgisayarın anlaması için
        // string name = "Mahmut";
        // int number = 2023;
        // float numberFloat = 3.14f;  //* float ifadelerin değerini(value'sunu) yazdıktan sonra "f"koymassak error verir
        // print(name);
        // print(number);


        //? ################### var kullanımı

        // var a = 3;          //* eğer float int veya string'le uğraşmak istemiyorsan böyle yapabilirsin
        // var b = 3.14f;
        // var c = "Mahmut Esat";

        // print(a);
        // print(b);
        // print(c);



        //? ################### Mathematicsss
        // print(107+2453);
        // print(10000000-2453);
        // print(55*354);          //* matematik modullerinin rahatça kullanabiliyoruz
        // print(5000/5);



        //? ################### boolean     (sadece 2 değer alabilir "true" and "false")
  
        // bool x = true;
        // bool y = false;

        // print(x);
        // print(y);


        //? ################### Stringlerle sayılar arasındaki temel fark

        // var x = 1;
        // var y = 2;

        // var x = "Mahmut ";      //* boşluk yazılım dillerinde bir karakterdir
        // var y = "Esat";

        // var x = "125 ";         //* "" çif tırnak içindeki herşey string olarak alınır 
        // var y = "256";

        // var a = "MEA";
        // var b = 5;
        // var c = 13.13f;

        // print(a + " " + b + " " + c);

        // a = "MEA";          //* tekrar var 'la a lara değişken atamaya çalışırsak unity error verir
        // b = 25;
        // c = 3.14f;

        // print(a + " " + b + " " + c);
        // print(x+y);


        //? ###################  compare operators
        //* biz bu operatörleri true yada false çıktısı almak için kullanırız

        //> - bigger than 
        // print(6>5);

        //< - smaller than
        // print(6<5);

        //==  - equal
        // print(6==6);

        //>= bigger or equal
        // print(4>=5);

        //<= smaller or equal
        // print(4<=5);

        
        //? ################### Koşullu durumlar ( if - else - else if )
        //* C# kodları yukarıdan aşağıya doğru okumaya başlar doğru koşulu bulduğu an devamını okumaz
        // var a = 5;
        // var b = 6;      //* tanımlamalarımızı(değişken tanımlamalarımızı) yukarıda yapmamız lazım 
        // var c = 5; 

        // if (a==b)   //* eğer koşulumuz doğruysa if'in altındakiler yapılır
        // {
        //     print("a is equal to b.");
        // }else if (a==c)    //* birden fazla koşul yazmak istiyorsak else if kullanıyoruz
        // {
        //     print("a is equal to c.");
        // }else       //* eğer koşulumuz yanlışsa else'in altındakiler yapılır
        // {
        //     print("a is not equal to b.");
        // }






        //? ################### while döngüsü
        //* eğer kod doğruysa sonsuz bir döngüye sokar eğer durması için herhangi bir şey yapmadıysak

        // var x = 0;

        // while (x < 5)   //* x beşe ulaşana kadar devam eder eğer kod (true çıktısı verirse)
        // {
        //     x = x + 1;
        //     print(x);

        // }




        //? ################### do-while döngüsü

        // var x = 0;

        // do    //* önce do işlemini yapar daha sonra while koşulu doğrumu kontrole gider dopruysa yapmaya
        // {
        //     x = x + 1;
            // * x +=1;   // şeklindede kullanabiliriz

        //     print(x);
        // }while( x < 5);




        //? ################### switch-case yapısı 
        //* switch case ' de sadece tek bir durum(case) doğru(true) doğru olabilir ve onu gerçekleştirir 

        // switch(x)   //* x'i al diyoruz bu kısımda ve kontrol et
        // {
        //     case 1:print("The value of x is 1.");       //* x == 1 ise kod çalışır değilse ikinci durum(case)'a geçer
        //         break;      //* x == 1 ise bundan sonraki kodu okuma demek

        //     case 2:print("The value of x is 2.");
        //         break;

        // }




        //? ################### for döngüsü
        //* belirli bir limite kadar döngü kurmamızı sağlar

        int x;   //* böyle bir değişkende oluşturabiliyoruz x'in int olduğunu söylüyoruz lakin şu an herhangi bir değeri olmadığını söylüyoruz

        for(x = 0; x <5; x+=1) //*  ( değişken tanımlıyoruz; koşulumuzu yazıyoruz; değişkenimizin nasıl davranması gerektiğini yazıyoruz)
        {
            print(x);
        }



        //? ################### Listeler  // birden fazla değeri içinde tutmamızı sağlıyor
        // List<string> days = new List<string>();   //* bu şekilde listeler oluşturabiliyoruz

        // days.add("Monday");
        // days.add("Tuesday");
        // days.add("Wednesday");
        // days.add("Thursday");

        // print(days);   //* list bir iterable objedir print() ile bastıramayız bunu foreach gibi methotlarla yapabiliriz bunu


        //? ################### foreach döngüsü

        // foreach (var item in days)   //* item diye bir değişken oluşturuyoruz ve günler adlı listemizin içinde sırasıyla gezinicek
        // {
        //     print(item);
        // }
    




        //? ################### Array 
        // string name = "Mahmut";

        // string[] days = {"Friday", "Saturday","Sunday"};


        // foreach(var item in days)
        // {
        //     print(item);
        // }


        // print(days[0]);   //* herhangi bir indexi böyle seçebiliyoruz like others languages




        //? ################### ArrayList

        // ArrayList complex = new ArrayList();

        // complex.Add(2);
        // complex.Add("Mahmut");
        // complex.Add(3.14f);

        // foreach(var item in complex)
        // {
        //     print(item);
        // }


        // print(complex[2]);
         






    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
