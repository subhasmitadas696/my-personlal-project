package app.ewarehouse.repository;

import org.springframework.data.jpa.repository.JpaRepository;

import app.ewarehouse.entity.TempUser;


public interface TempUserRepository extends JpaRepository<TempUser, Integer> {

	TempUser findByEmail(String email);
}
