Yapýlanlar

1. Data içindeki modeller core katmanýna taþýndý.
1. model sýnýflarýna marker amacý ile IModel interface sýnýfý baðlandý.
1. benim nefret ettiðim altý çizerek verilen uyarýlarý kaldýrmak için bütün stringlere `= null!;` eklendi.
1. Post için verilen veritabaný ayarýný Data katmanýnda Configs içine ekledim ayarlar toplu olmalý ama tüm ayarlarý yazamam üþeniyom vakitte az
1. Service hem core hemde data için referans almýþtý. sadece data olarak deðiþtrdim. Onion architecture mantýðý. onuda context içinde otomatik ekleyen kodu koyarak tamamladým.
1. serviste her seferinde context iþleri olmakta bu servis için `BANA GÖRE` uygun bir hareket deðil ondan dolayý ben repository katmaný eklemek istedim. yapýyýda bozamýyacaðým için Data içine koydum. Bu adýmda normalde dönüþ için özel sýnýflar kontroller loglar vs olur o kadar girmedim olan yapýya ek özellik eklemek yerine yapýyý sadece düzeltme çerçevesinde ekleme silme güncelleme yaptým.
1. üstteki maddede büyük konuþtum. Return tipi ekliyorum olumlu ve olumsuz dönüþler için string kullanýlmýþ düzelmesi lazým
1. User içinde Userpost adlý kolon kaldýrýldý. Bence performans ve veritabaný tasarýmý için mantýklý bir hareket deðil.
1. servislere consol çýktýsý olarak log niyetine yazý ekledim. 
1. repositoryler tüm servis iþini yüklenmekte
1. geri dönüþ için bir interface ile yönettim.
1. bazý fonksiyon isimlerini düzenledim
1. Diðer iþler için bana göre makul bir düzeltme evresinde projeyi tamamlýyorum. Çalýþtýrma ve test etme olmadý.






# Kod Deðiþim Örnekleri

``` 

///////Eski Kod\\\\\\\

 public async Task<List<Post>> GetPostsAndUsersAsync()
{
    var posts = await _dbContext.Posts.ToListAsync();
    var postsWithUsers = new List<Post>();

    foreach (var post in posts)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == post.UserId);
        post.User = user;
        postsWithUsers.Add(post);
    }

    return postsWithUsers;
}
```


```

///////Yeni Kod\\\\\\\

public async Task<List<Post>> GetPostsAndUsersAsync()
{
    return await context.Posts.Include(p => p.User).ToListAsync(); //kod iyileþtirildi.
}
```

`var result = ` dedikten sonra `return` etmem mantýklý ama gereksik görüyorum. Boþ yere deðiþken atamasý bana sýrf görüntü için gereksiz maliyet demektir.

<br>
<br>

büyük konuþmamak lazým string dönüþler için return tipi lazým oldu yani ben öyle çözdümn ondan kod aþaðýdaki hale döndü. 

```
public async Task<IReturn<List<Post>>> GetPostsAndUsersAsync()
{
    var result = await context.Posts.Include(p => p.User).ToListAsync(); //kod iyileþtirildi.
    return CheckIsNull(result);
}
```







