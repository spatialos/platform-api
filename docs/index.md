# Protocol Documentation
<a name="top"/>

## Table of Contents

- [player_identity.proto](#player_identity.proto)
    - [CreatePlayerTokenRequest](#improbable.spatialos.playerauth.v1alpha1.CreatePlayerTokenRequest)
    - [CreatePlayerTokenResponse](#improbable.spatialos.playerauth.v1alpha1.CreatePlayerTokenResponse)
    - [PlayerToken](#improbable.spatialos.playerauth.v1alpha1.PlayerToken)
  
  
  
    - [PlayerTokenService](#improbable.spatialos.playerauth.v1alpha1.PlayerTokenService)
  

- [Scalar Value Types](#scalar-value-types)



<a name="player_identity.proto"/>
<p align="right"><a href="#top">Top</a></p>

## player_identity.proto



<a name="improbable.spatialos.playerauth.v1alpha1.CreatePlayerTokenRequest"/>

### CreatePlayerTokenRequest



| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| player_identifier | [string](#string) |  | Player identifier, eg: username |
| project_name | [string](#string) |  | Name of the project player_identifier should access |






<a name="improbable.spatialos.playerauth.v1alpha1.CreatePlayerTokenResponse"/>

### CreatePlayerTokenResponse



| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| token | [PlayerToken](#improbable.spatialos.playerauth.v1alpha1.PlayerToken) |  | Token granting the player access to the requested project |






<a name="improbable.spatialos.playerauth.v1alpha1.PlayerToken"/>

### PlayerToken



| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| token | [string](#string) |  |  |





 

 

 


<a name="improbable.spatialos.playerauth.v1alpha1.PlayerTokenService"/>

### PlayerTokenService
PlayerAuthenticationService provides methods for generation of player authentication.

| Method Name | Request Type | Response Type | Description |
| ----------- | ------------ | ------------- | ------------|
| CreatePlayerToken | [CreatePlayerTokenRequest](#improbable.spatialos.playerauth.v1alpha1.CreatePlayerTokenRequest) | [CreatePlayerTokenResponse](#improbable.spatialos.playerauth.v1alpha1.CreatePlayerTokenRequest) | Returns a new token required to access the requested project |

 



## Scalar Value Types

| .proto Type | Notes | C++ Type | Java Type | Python Type |
| ----------- | ----- | -------- | --------- | ----------- |
| <a name="double" /> double |  | double | double | float |
| <a name="float" /> float |  | float | float | float |
| <a name="int32" /> int32 | Uses variable-length encoding. Inefficient for encoding negative numbers – if your field is likely to have negative values, use sint32 instead. | int32 | int | int |
| <a name="int64" /> int64 | Uses variable-length encoding. Inefficient for encoding negative numbers – if your field is likely to have negative values, use sint64 instead. | int64 | long | int/long |
| <a name="uint32" /> uint32 | Uses variable-length encoding. | uint32 | int | int/long |
| <a name="uint64" /> uint64 | Uses variable-length encoding. | uint64 | long | int/long |
| <a name="sint32" /> sint32 | Uses variable-length encoding. Signed int value. These more efficiently encode negative numbers than regular int32s. | int32 | int | int |
| <a name="sint64" /> sint64 | Uses variable-length encoding. Signed int value. These more efficiently encode negative numbers than regular int64s. | int64 | long | int/long |
| <a name="fixed32" /> fixed32 | Always four bytes. More efficient than uint32 if values are often greater than 2^28. | uint32 | int | int |
| <a name="fixed64" /> fixed64 | Always eight bytes. More efficient than uint64 if values are often greater than 2^56. | uint64 | long | int/long |
| <a name="sfixed32" /> sfixed32 | Always four bytes. | int32 | int | int |
| <a name="sfixed64" /> sfixed64 | Always eight bytes. | int64 | long | int/long |
| <a name="bool" /> bool |  | bool | boolean | boolean |
| <a name="string" /> string | A string must always contain UTF-8 encoded or 7-bit ASCII text. | string | String | str/unicode |
| <a name="bytes" /> bytes | May contain any arbitrary sequence of bytes. | string | ByteString | str |

