# DoCert
Docert (DOnation CERTificate manager) je aplikace pro správu potvrzení o darech.

## Stažení a instalace
Verze pro Windows x64 ke stažení zde: <a href="https://www.skrobak.net/download/docert.zip">docert.zip</a>

### Instalace
Adresář docert ze souboru docert.zip rozbalte kamkoliv na disk a spusťte docert.exe.

## Jak na to?

### Dárci
Nejprve budete potřebovat seznam dárců.

Můžete je zadat ručně nebo importovat z Excelu.

Aby bylo možno dárce spárovat s platbou, musí mít dárce vyplněno číslo účtu (pro jednoduchost a univerzálnost doporučujeme použít IBAN) nebo správný variabilní symbol (VS).

Datum narození je zase nutný pro identifikaci dárce pro potřeby finančního úřadu.

### Dary
Tabulka darů obsahuje seznam přijatých plateb.

Opět je můžete zadat ručně nebo importovat z Excelu. Excelovou tabulku si snadno můžete připravit z elektronického výpisu z bankovního účtu.

Aplikace předpokládá, že seznam darů obsahuje platby za minulý rok.

### Potvrzení
Jakmile máte připravená data o dárcích a darech, můžete vytvářet potvrzení.

Import potvrzení zpracuje data a vygeneruje automaticky potvrzení o darech za minulý rok. Každé potvrzení lze manuálně upravit. Také lze vytvořit potvrzení ručně (pokud např. chcete rozdělit dary mezi manžely apod.).

Potvrzení můžete uložit od pdf nebo odeslat emailem a to jak po jednom, tak hromadně.


## Kam se ukládají data?
Data o darech jsou citlivá a proto je aplikace navržena tak, aby data neopustila počítač uživatele. Aplikace pracuje s databázovým souborem, který je uložen v profilu uživatele mimo adresář aplikace.

c:\Users\[uživatel]\AppData\Roaming\DoCert\docert.db

Při prvním spuštění se vytvoří nový databázový soubor docert.db (pokud již v profilu uživatele neexistuje z předchozího použití aplikace). Se souborem se dá manipulovat při vypnuté aplikaci, např. zálohovat, přesouvat na jiný počítač apod.


## Nastavení

### Grafika (logo, razítko a podpis)
...

### E-mail
...
