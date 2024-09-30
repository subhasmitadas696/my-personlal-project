package app.ewarehouse.config;


import java.util.Base64;

import org.json.JSONObject;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.context.SecurityContext;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.web.authentication.WebAuthenticationDetailsSource;
import org.springframework.stereotype.Component;
import org.springframework.web.filter.OncePerRequestFilter;

import app.ewarehouse.exception.JwtTimeOutException;
import app.ewarehouse.service.JwtService;
import app.ewarehouse.service.UserService;
import io.micrometer.common.util.StringUtils;
import jakarta.servlet.FilterChain;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import lombok.RequiredArgsConstructor;


@Component
@RequiredArgsConstructor
public class JwtAuthenticationFilter extends OncePerRequestFilter{
	
	@Autowired
	private final JwtService jwtService;
	
	@Autowired
	private final UserService userService;
	
	
	@Override
	protected void doFilterInternal(HttpServletRequest request, HttpServletResponse response, FilterChain filterChain){
		
		try {
			final String authHeader = request.getHeader("Authorization");
			final String jwt;
			final String userEmail;
			if(authHeader == null || ! authHeader.startsWith("Bearer ")) {
				filterChain.doFilter(request, response);
				
			}
			
			else {
			
				jwt = authHeader.substring(7);
				
				String[] chunks = jwt.split("\\.");
				
				Base64.Decoder decoder = Base64.getUrlDecoder();
				String header = new String(decoder.decode(chunks[0]));
				String payload = new String(decoder.decode(chunks[1]));
				
				  JSONObject jsonobj=new JSONObject(payload);
				  
				  String roleId=jsonobj.get("roleId").toString();
				  String userid=jsonobj.get("userid").toString();
				  String email=jsonobj.get("email").toString();
				  String username=jsonobj.get("username").toString();
				 
				  request.setAttribute("roleId", roleId);
				  request.setAttribute("email", email);
				  request.setAttribute("userid", userid);
				  request.setAttribute("username", username);
				  
				userEmail = jwtService.extractUserName(jwt);
				if (StringUtils.isNotEmpty(userEmail) && SecurityContextHolder.getContext().getAuthentication() == null) {
					UserDetails userDetails = userService.userDetailsService().loadUserByUsername(userEmail);
					if (jwtService.isTokenValid(jwt, userDetails)) {
						SecurityContext securityContext = SecurityContextHolder.createEmptyContext();
						UsernamePasswordAuthenticationToken token = new UsernamePasswordAuthenticationToken(userDetails, null,userDetails.getAuthorities());
						token.setDetails(new WebAuthenticationDetailsSource().buildDetails(request));
						securityContext.setAuthentication(token);
						SecurityContextHolder.setContext(securityContext);
					}
				}
				
				System.out.println("token received"+jwt);
				filterChain.doFilter(request, response);
				
			}
		}
		catch(Exception e){
			
			 throw new JwtTimeOutException("Jwt has expired or authentication failuer");
		}
		 
	}

}
