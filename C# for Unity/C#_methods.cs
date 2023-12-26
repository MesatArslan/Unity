using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
        //? ################### Start and update fonksiyonları
        //* start methodu console'a bir kere yadırır
        //* update methodu kullanılırsa  her frame(sahne)'de tekrar tekrar yazdırılır    (örnek filmlerde genelde 24 frame kullanılır saniyede)

        // void sum(7,6);    //* methodları böyle çağırıyoruz
        // nameAndSurname("Mahmut Esat", "Arslan");
        printScreen(name("Mahmut Esat Arslan"));
    
    }


    //? ################### void methodlar (void fonksiyonumuzu oluşturmak için)
    //* methodun oluşturulması çağırıldığı anlamına gelmez bunun için tekrar bir kkomut vermemiz gerekir
    //* void sum();    //* void methodları böyle çağırıyoruz
    

    // void sum(int x, int y) //* parantezin içine yazdığımız değer ler şu anlama geliyor, ben sana 2 integer değer vericem sen bunları al ve topla
    // {
    //     print(x+y);
        
    // }

    // void nameAndSurname(string name , string surname)
    // {
    //     printi(name + surname);
    // }



    //? ################### void olmayan methodlar (void fonksiyonumuzu oluşturmak için)
    //* void fonksiyonlarda şunu yyapamıyoruz sum(substract()); bir void fonksiyondan aldığımız değeri başka bir void fondsiyona veremiyoruz

    void substract(int number)  //* void olanı başka bir void fonksiyona veremiyoruz
    {
        print(number - 5);
    }

    int sum(int x , int y)
    {
        return x+y;  //* return içinde yaptığı işlemi tutmamızı sağlıyor
    }

    string name(string name)
    {
        return name;   //* void olmayan fonksiyonlar sadece return kullanabiliyor(yani değer tutabiliyor)
    }

    void printScreen(string name)
    {
        print(name);
    }




    // Update is called once per frame
    void Update()
    {
        print(5+4);
        
    }
}
