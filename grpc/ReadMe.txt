//https://learn.microsoft.com/en-us/dotnet/architecture/grpc-for-wcf-developers/protobuf-data-types

// important! dont forget to use import
// for example for DateTime
// import "google/protobuf/timestamp.proto";



Protobuf	type 	C# type 	Notes
double 		double 	
float 		float 	
int32		int 		1
int64 		long		1
uint32		uint 	
uint64 		ulong 	
sint32		int 		1
sint64		long 		1
fixed32 	uint		2
fixed64 	ulong		2
sfixed32 	int 		2
sfixed64 	long 		2
bool 		bool 	
string		string 		3
bytes 		ByteString 	4



// Make propery nullable
import "google/protobuf/wrappers.proto";

 google.protobuf.StringValue firstName = 1;
 google.protobuf.Int32Value age = 2;