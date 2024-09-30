package app.ewarehouse.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import app.ewarehouse.entity.Seasonality;

@Repository
public interface SeasonalityRepository extends JpaRepository<Seasonality, Integer>{

}
