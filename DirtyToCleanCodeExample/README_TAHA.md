Yap�lanlar

1. Data i�indeki modeller core katman�na ta��nd�.
1. model s�n�flar�na marker amac� ile IModel interface s�n�f� ba�land�.
1. benim nefret etti�im alt� �izerek verilen uyar�lar� kald�rmak i�in b�t�n stringlere `= null!;` eklendi.
1. Post i�in verilen veritaban� ayar�n� Data katman�nda Configs i�ine ekledim ayarlar toplu olmal� ama t�m ayarlar� yazamam ��eniyom vakitte az
1. Service hem core hemde data i�in referans alm��t�. sadece data olarak de�i�trdim. Onion architecture mant���. onuda context i�inde otomatik ekleyen kodu koyarak tamamlad�m.
1. serviste her seferinde context i�leri olmakta bu servis i�in `BANA G�RE` uygun bir hareket de�il ondan dolay� ben repository katman� eklemek istedim. yap�y�da bozam�yaca��m i�in Data i�ine koydum. Bu ad�mda normalde d�n�� i�in �zel s�n�flar kontroller loglar vs olur o kadar girmedim olan yap�ya ek �zellik eklemek yerine yap�y� sadece d�zeltme �er�evesinde ekleme silme g�ncelleme yapt�m.
1. �stteki maddede b�y�k konu�tum. Return tipi ekliyorum olumlu ve olumsuz d�n��ler i�in string kullan�lm�� d�zelmesi laz�m
1. User i�inde Userpost adl� kolon kald�r�ld�. Bence performans ve veritaban� tasar�m� i�in mant�kl� bir hareket de�il.
1. servislere consol ��kt�s� olarak log niyetine yaz� ekledim. 
1. repositoryler t�m servis i�ini y�klenmekte
1. geri d�n�� i�in bir interface ile y�nettim.
1. baz� fonksiyon isimlerini d�zenledim
1. Di�er i�ler i�in bana g�re makul bir d�zeltme evresinde projeyi tamaml�yorum. �al��t�rma ve test etme olmad�.






# Kod De�i�im �rnekleri

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
    return await context.Posts.Include(p => p.User).ToListAsync(); //kod iyile�tirildi.
}
```

`var result = ` dedikten sonra `return` etmem mant�kl� ama gereksik g�r�yorum. Bo� yere de�i�ken atamas� bana s�rf g�r�nt� i�in gereksiz maliyet demektir.

<br>
<br>

b�y�k konu�mamak laz�m string d�n��ler i�in return tipi laz�m oldu yani ben �yle ��zd�mn ondan kod a�a��daki hale d�nd�. 

```
public async Task<IReturn<List<Post>>> GetPostsAndUsersAsync()
{
    var result = await context.Posts.Include(p => p.User).ToListAsync(); //kod iyile�tirildi.
    return CheckIsNull(result);
}
```







