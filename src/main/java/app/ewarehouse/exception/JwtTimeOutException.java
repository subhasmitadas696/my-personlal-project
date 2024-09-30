package app.ewarehouse.exception;



public class JwtTimeOutException extends RuntimeException{

   public JwtTimeOutException(){
        super("Token time out");
    }

    public JwtTimeOutException(String message){
        super(message);
    }

}
