³

NH:\AlphaMedic\AlphaMedic\AuthorizationServer\Controllers\AudienceController.cs
	namespace 	
AuthorizationServer
 
. 
Controllers )
{ 
[ 
RoutePrefix 
( 
$str 
)  
]  !
public 

class 
AudienceController #
:$ %
ApiController& 3
{		 
[

 	
Route

	 
(

 
$str

 
)

 
]

 
public 
IHttpActionResult  
Post! %
(% &
AudienceModel& 3
audienceModel4 A
)A B
{ 	
if 
( 
! 

ModelState 
. 
IsValid #
)# $
{ 
return 

BadRequest !
(! "

ModelState" ,
), -
;- .
} 
Audience 
newAudience  
=! "
AudiencesStore# 1
.1 2
AddAudience2 =
(= >
audienceModel> K
.K L
NameL P
)P Q
;Q R
return 
Ok 
( 
newAudience !
)! "
;" #
} 	
} 
} ­	
AH:\AlphaMedic\AlphaMedic\AuthorizationServer\Entities\Audience.cs
	namespace 	
AuthorizationServer
 
. 
Entities &
{ 
public 

class 
Audience 
{ 
[ 	
Key	 
] 
[ 	
	MaxLength	 
( 
$num 
) 
] 
public		 
string		 
ClientId		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		+ ,
[ 	
	MaxLength	 
( 
$num 
) 
] 
[ 	
Required	 
] 
public 
string 
Base64Secret "
{# $
get% (
;( )
set* -
;- .
}/ 0
[ 	
	MaxLength	 
( 
$num 
) 
] 
[ 	
Required	 
] 
public 
string 
Name 
{ 
get  
;  !
set" %
;% &
}' (
} 
} Ê
DH:\AlphaMedic\AlphaMedic\AuthorizationServer\Models\AudienceModel.cs
	namespace 	
AuthorizationServer
 
. 
Models $
{ 
public 

class 
AudienceModel 
{ 
[ 	
	MaxLength	 
( 
$num 
) 
] 
[ 	
Required	 
] 
public		 
string		 
Name		 
{		 
get		  
;		  !
set		" %
;		% &
}		' (
}

 
} ×
DH:\AlphaMedic\AlphaMedic\AuthorizationServer\Models\AudienceStore.cs
	namespace

 	
AuthorizationServer


 
.

 
Models

 $
{ 
public 

static 
class 
AudiencesStore &
{ 
public 
static  
ConcurrentDictionary *
<* +
string+ 1
,1 2
Audience3 ;
>; <
AudiencesList= J
=K L
new  
ConcurrentDictionary $
<$ %
string% +
,+ ,
Audience- 5
>5 6
(6 7
)7 8
;8 9
static 
AudiencesStore 
( 
) 
{ 	
AudiencesList 
. 
TryAdd  
(  !
$str! C
,C D
new  #
Audience$ ,
{  !
ClientId$ ,
=- .
$str/ Q
,Q R
Base64Secret$ 0
=1 2
$str3 `
,` a
Name$ (
=) *
$str+ 1
}  !
)! "
;" #
} 	
public 
static 
Audience 
AddAudience *
(* +
string+ 1
name2 6
)6 7
{ 	
var 
clientId 
= 
Guid 
.  
NewGuid  '
(' (
)( )
.) *
ToString* 2
(2 3
$str3 6
)6 7
;7 8
var   
key   
=   
new   
byte   
[   
$num   !
]  ! "
;  " #$
RNGCryptoServiceProvider!! $
.!!$ %
Create!!% +
(!!+ ,
)!!, -
.!!- .
GetBytes!!. 6
(!!6 7
key!!7 :
)!!: ;
;!!; <
var"" 
base64Secret"" 
="" 
TextEncodings"" ,
."", -
	Base64Url""- 6
.""6 7
Encode""7 =
(""= >
key""> A
)""A B
;""B C
Audience$$ 
newAudience$$  
=$$! "
new$$# &
Audience$$' /
{%% 
ClientId&& 
=&& 
clientId&& #
,&&# $
Base64Secret'' 
='' 
base64Secret'' +
,''+ ,
Name(( 
=(( 
name(( 
})) 
;)) 
AudiencesList** 
.** 
TryAdd**  
(**  !
clientId**! )
,**) *
newAudience**+ 6
)**6 7
;**7 8
return++ 
newAudience++ 
;++ 
},, 	
public.. 
static.. 
Audience.. 
FindAudience.. +
(..+ ,
string.., 2
clientId..3 ;
)..; <
{// 	
Audience00 
audience00 
=00 
null00  $
;00$ %
if11 
(11 
AudiencesList11 
.11 
TryGetValue11 )
(11) *
clientId11* 2
,112 3
out114 7
audience118 @
)11@ A
)11A B
{22 
return33 
audience33 
;33  
}44 
return55 
null55 
;55 
}66 	
}77 
}88 ¬
@H:\AlphaMedic\AlphaMedic\AuthorizationServer\Models\UserStore.cs
	namespace 	
AuthorizationServer
 
. 
Models $
{ 
public		 

class		 
	UserStore		 
{

 
AlphaMedicContext 
db 
= 
new "
AlphaMedicContext# 4
(4 5
)5 6
;6 7
} 
} ò
GH:\AlphaMedic\AlphaMedic\AuthorizationServer\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str .
). /
]/ 0
[		 
assembly		 	
:			 

AssemblyDescription		 
(		 
$str		 !
)		! "
]		" #
[

 
assembly

 	
:

	 
!
AssemblyConfiguration

  
(

  !
$str

! #
)

# $
]

$ %
[ 
assembly 	
:	 

AssemblyCompany 
( 
$str 
) 
] 
[ 
assembly 	
:	 

AssemblyProduct 
( 
$str 0
)0 1
]1 2
[ 
assembly 	
:	 

AssemblyCopyright 
( 
$str 0
)0 1
]1 2
[ 
assembly 	
:	 

AssemblyTrademark 
( 
$str 
)  
]  !
[ 
assembly 	
:	 

AssemblyCulture 
( 
$str 
) 
] 
[ 
assembly 	
:	 


ComVisible 
( 
false 
) 
] 
[ 
assembly 	
:	 

Guid 
( 
$str 6
)6 7
]7 8
["" 
assembly"" 	
:""	 

AssemblyVersion"" 
("" 
$str"" $
)""$ %
]""% &
[## 
assembly## 	
:##	 

AssemblyFileVersion## 
(## 
$str## (
)##( )
]##) *¼3
MH:\AlphaMedic\AlphaMedic\AuthorizationServer\Providers\CustomOAuthProvider.cs
	namespace 	
AuthorizationServer
 
. 
	Providers '
{ 
public 

class 
CustomOAuthProvider $
:% &,
 OAuthAuthorizationServerProvider' G
{ 
public 
override 
Task (
ValidateClientAuthentication 9
(9 :4
(OAuthValidateClientAuthenticationContext 4
context5 <
)< =
{ 	
string 
clientId 
= 
string $
.$ %
Empty% *
;* +
string 
clientSecret 
=  !
string" (
.( )
Empty) .
;. /
string  
symmetricKeyAsBase64 '
=( )
string* 0
.0 1
Empty1 6
;6 7
if 
( 
! 
context 
. "
TryGetBasicCredentials /
(/ 0
out0 3
clientId4 <
,< =
out> A
clientSecretB N
)N O
)O P
{ 
context 
. !
TryGetFormCredentials -
(- .
out. 1
clientId2 :
,: ;
out< ?
clientSecret@ L
)L M
;M N
} 
if 
( 
context 
. 
ClientId  
==! #
null$ (
)( )
{ 
context 
. 
SetError  
(  !
$str! 3
,3 4
$str5 K
)K L
;L M
return   
Task   
.   

FromResult   &
<  & '
object  ' -
>  - .
(  . /
null  / 3
)  3 4
;  4 5
}!! 
var## 
audience## 
=## 
AudiencesStore## )
.##) *
FindAudience##* 6
(##6 7
context##7 >
.##> ?
ClientId##? G
)##G H
;##H I
if%% 
(%% 
audience%% 
==%% 
null%%  
)%%  !
{&& 
context'' 
.'' 
SetError''  
(''  !
$str(( &
,((& '
string)) 
.)) 
Format)) !
())! "
$str))" ;
,)); <
context))= D
.))D E
ClientId))E M
)))M N
)** 
;** 
return++ 
Task++ 
.++ 

FromResult++ &
<++& '
object++' -
>++- .
(++. /
null++/ 3
)++3 4
;++4 5
},, 
context.. 
... 
	Validated.. 
(.. 
).. 
;..  
return// 
Task// 
.// 

FromResult// "
<//" #
object//# )
>//) *
(//* +
null//+ /
)/// 0
;//0 1
}00 	
public22 
override22 
async22 
Task22 ")
GrantResourceOwnerCredentials22# @
(22@ A5
)OAuthGrantResourceOwnerCredentialsContext33 5
context336 =
)33= >
{44 	
context66 
.66 
OwinContext66 
.66  
Response66  (
.66( )
Headers66) 0
.660 1
Add661 4
(664 5
$str665 R
,66R S
new66T W
[66W X
]66X Y
{66Z [
$str66\ _
}66` a
)66a b
;66b c
var88 
userProvider88 
=88 
new88 "
UserProvider88# /
(88/ 0
new880 3
Rest884 8
.888 9
Models889 ?
.88? @
AlphaMedicContext88@ Q
.88Q R
AlphaMedicContext88R c
(88c d
)88d e
)88e f
;88f g
var99 
user99 
=99 
await99 
userProvider99 )
.99) *
FindByEmailAsync99* :
(99: ;
context99; B
.99B C
UserName99C K
)99K L
;99L M
if<< 
(<< 
user<< 
==<< 
null<< 
||<< 
user<<  $
.<<$ %
Password<<% -
!=<<. 0
context<<1 8
.<<8 9
Password<<9 A
)<<A B
{== 
context>> 
.>> 
SetError>>  
(>>  !
$str?? #
,??# $
$str@@ =
)AA 
;AA 
contextBB 
.BB 
RejectedBB  
(BB  !
)BB! "
;BB" #
returnCC 
;CC 
}DD 
varFF 
identityFF 
=FF 
newFF 
ClaimsIdentityFF -
(FF- .
$strFF. 3
)FF3 4
;FF4 5
identityHH 
.HH 
AddClaimHH 
(HH 
newHH !
ClaimHH" '
(HH' (

ClaimTypesHH( 2
.HH2 3
NameHH3 7
,HH7 8
contextHH9 @
.HH@ A
UserNameHHA I
)HHI J
)HHJ K
;HHK L
identityII 
.II 
AddClaimII 
(II 
newII !
ClaimII" '
(II' (
$strII( -
,II- .
contextII/ 6
.II6 7
UserNameII7 ?
)II? @
)II@ A
;IIA B
varTT 
propsTT 
=TT 
newTT $
AuthenticationPropertiesTT 4
(TT4 5
newTT5 8

DictionaryTT9 C
<TTC D
stringTTD J
,TTJ K
stringTTL R
>TTR S
{UU 
{VV 
$strWW #
,WW# $
(WW% &
contextWW& -
.WW- .
ClientIdWW. 6
==WW7 9
nullWW: >
)WW> ?
?WW@ A
stringWWB H
.WWH I
EmptyWWI N
:WWO P
contextWWQ X
.WWX Y
ClientIdWWY a
}XX 
}YY 
)YY 
;YY 
var[[ 
ticket[[ 
=[[ 
new[[  
AuthenticationTicket[[ 1
([[1 2
identity[[2 :
,[[: ;
props[[< A
)[[A B
;[[B C
context\\ 
.\\ 
	Validated\\ 
(\\ 
ticket\\ $
)\\$ %
;\\% &
}]] 	
}^^ 
}__ ñ
7H:\AlphaMedic\AlphaMedic\AuthorizationServer\Startup.cs
[ 
assembly 	
:	 

OwinStartup 
( 
typeof 
( 
Startup %
)% &
)& '
]' (
	namespace 	
AuthorizationServer
 
{ 
public 

class 
Startup 
{ 
public 
void 
Configuration !
(! "
IAppBuilder" -
app. 1
)1 2
{ 	
HttpConfiguration 
config $
=% &
new' *
HttpConfiguration+ <
(< =
)= >
;> ?
config 
. "
MapHttpAttributeRoutes )
() *
)* +
;+ ,
ConfigureOAuth 
( 
app 
) 
;  
app 
. 
UseCors 
( 
	Microsoft !
.! "
Owin" &
.& '
Cors' +
.+ ,
CorsOptions, 7
.7 8
AllowAll8 @
)@ A
;A B
app 
. 
	UseWebApi 
( 
config  
)  !
;! "
} 	
public 
void 
ConfigureOAuth "
(" #
IAppBuilder# .
app/ 2
)2 3
{   	+
OAuthAuthorizationServerOptions"" +
OAuthServerOptions"", >
=""? @
new""A D+
OAuthAuthorizationServerOptions""E d
(""d e
)""e f
{## 
AllowInsecureHttp%% !
=%%" #
true%%$ (
,%%( )
TokenEndpointPath&& !
=&&" #
new&&$ '

PathString&&( 2
(&&2 3
$str&&3 ;
)&&; <
,&&< =%
AccessTokenExpireTimeSpan'' )
=''* +
TimeSpan'', 4
.''4 5
FromMinutes''5 @
(''@ A
$num''A C
)''C D
,''D E
}** 
;** 
app-- 
.-- '
UseOAuthAuthorizationServer-- +
(--+ ,
OAuthServerOptions--, >
)--> ?
;--? @
}// 	
}00 
}11 